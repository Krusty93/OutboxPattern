using System;
using System.Text.Json.Serialization;

namespace OutboxPattern.FunctionApp;

public record After(
    [property: JsonPropertyName("Id")] Guid Id,
    [property: JsonPropertyName("OccurredOn")] long OccurredOn,
    [property: JsonPropertyName("Type")] string Type,
    [property: JsonPropertyName("Data")] string Data,
    [property: JsonPropertyName("ProcessedDate")] long? ProcessedDate
);

public record EventHubOutboxMessage(
    [property: JsonPropertyName("before")] object Before,
    [property: JsonPropertyName("after")] After After,
    [property: JsonPropertyName("source")] EventhubSource Source,
    [property: JsonPropertyName("op")] string Op,
    [property: JsonPropertyName("ts_ms")] long TsMs,
    [property: JsonPropertyName("transaction")] object Transaction
);

public record EventhubSource(
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("connector")] string Connector,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("ts_ms")] long TsMs,
    [property: JsonPropertyName("snapshot")] string Snapshot,
    [property: JsonPropertyName("db")] string Db,
    [property: JsonPropertyName("sequence")] object Sequence,
    [property: JsonPropertyName("schema")] string Schema,
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("change_lsn")] string ChangeLsn,
    [property: JsonPropertyName("commit_lsn")] string CommitLsn,
    [property: JsonPropertyName("event_serial_no")] int EventSerialNo
);

public record OutboxMessageDomain(
    [property: JsonPropertyName("Id")] Guid Id,
    [property: JsonPropertyName("OccurredOn")] DateTime OccurredOn,
    [property: JsonPropertyName("Type")] string Type,
    [property: JsonPropertyName("Data")] string Data,
    [property: JsonPropertyName("ProcessedDate")] DateTime? ProcessedDate
);
