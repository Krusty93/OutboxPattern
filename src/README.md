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

### Connector configuration
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