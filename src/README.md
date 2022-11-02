### SQL Change data capture

#### Enable
```sql
EXEC sys.sp_cdc_enable_table N'dbo', N'OutboxMessages', @role_name=null, @supports_net_changes=0'
-- columns: , @captured_column_list='[Id], [OccurredOn], [Type], [Data]
```

#### Disable
```sql
EXEC sys.sp_cdc_disable_table N'dbo', N'OutboxMessages', @capture_instance='dbo_OutboxMessages'
```

#### Check config
```sql
EXEC sys.sp_cdc_help_change_data_capture
```

### SQL Script testing

```sql
INSERT INTO [dbo].[OutboxMessages]
VALUES(NEWID(), GETDATE(), 'OrderPlacedIntegrationEvent', '{"OrderId": "123"}', NULL)

SELECT *
FROM [dbo].[OutboxMessages]
```

```sql
UPDATE [dbo].[OutboxMessages]
SET [Data] = 'new data'

DELETE FROM [dbo].[OutboxMessages]
```

### Debezium

#### Deploy
```bash
az container create -g $RESOURCE_GROUP -n $CONTAINER_NAME \
    --image debezium/connect:${DEBEZIUM_VERSION} \
    --ports 8083 --ip-address Public \
    --os-type Linux --cpu 2 --memory 4 \
    --environment-variables \
        BOOTSTRAP_SERVERS=${EH_NAME}.servicebus.windows.net:9093 \
        GROUP_ID=1 \
        CONFIG_STORAGE_TOPIC=debezium_configs \
        OFFSET_STORAGE_TOPIC=debezium_offsets \
        STATUS_STORAGE_TOPIC=debezium_statuses \
        CONNECT_KEY_CONVERTER_SCHEMAS_ENABLE=false \
        CONNECT_VALUE_CONVERTER_SCHEMAS_ENABLE=true \
        CONNECT_REQUEST_TIMEOUT_MS=60000 \
        CONNECT_SECURITY_PROTOCOL=SASL_SSL \
        CONNECT_SASL_MECHANISM=PLAIN \
        CONNECT_SASL_JAAS_CONFIG="org.apache.kafka.common.security.plain.PlainLoginModule required username=\"\$ConnectionString\" password=\"${EH_CONNECTION_STRING}\";" \
        CONNECT_PRODUCER_SECURITY_PROTOCOL=SASL_SSL \
        CONNECT_PRODUCER_SASL_MECHANISM=PLAIN \
        CONNECT_PRODUCER_SASL_JAAS_CONFIG="org.apache.kafka.common.security.plain.PlainLoginModule required username=\"\$ConnectionString\" password=\"${EH_CONNECTION_STRING}\";" \
        CONNECT_CONSUMER_SECURITY_PROTOCOL=SASL_SSL \
        CONNECT_CONSUMER_SASL_MECHANISM=PLAIN \
        CONNECT_CONSUMER_SASL_JAAS_CONFIG="org.apache.kafka.common.security.plain.PlainLoginModule required username=\"\$ConnectionString\" password=\"${EH_CONNECTION_STRING}\";"
```

#### Connector configuration
```json
{
    "name": "outbox",
    "config": {
        "snapshot.mode": "schema_only",
        "connector.class": "io.debezium.connector.sqlserver.SqlServerConnector",
        "database.hostname": "debezium-flowe.database.windows.net",
        "database.port": "1433",
        "database.user": "debezium-wwi",
        "database.password": "Password1!",
        "database.dbname": "OutboxPattern",
        "database.server.name": "SQLAzure",
        "tasks.max": "1",
        "decimal.handling.mode": "string",
        "table.include.list": "dbo.OutboxMessages",
        "transforms": "Reroute",
        "transforms.Reroute.type": "io.debezium.transforms.ByLogicalTableRouter",
        "transforms.Reroute.topic.regex": "(.*)",
        "transforms.Reroute.topic.replacement": "wwi",
        "tombstones.on.delete": false,
        "database.history": "io.debezium.relational.history.MemoryDatabaseHistory"
    }
}
```

#### Change Feed message example
```json
{
    "before": null,
    "after": {
        "Id": "9387F493-02B3-4914-9A11-80D8B7F6A2E2",
        "OccurredOn": 1666952973063333300,
        "Type": "OrderPlacedIntegrationEvent",
        "Data": "{"OrderId": "123"}",
        "ProcessedDate": null
    },
    "source": {
        "version": "1.6.3.Final",
        "connector": "sqlserver",
        "name": "SQLAzure",
        "ts_ms": 1666952973063,
        "snapshot": "false",
        "db": "OutboxPattern",
        "sequence": null,
        "schema": "dbo",
        "table": "OutboxMessages",
        "change_lsn": "0000002c:00000b28:0004",
        "commit_lsn": "0000002c:00000b28:0005",
        "event_serial_no": 1
    },
    "op": "c",
    "ts_ms": 1666952980699,
    "transaction": null
}
```