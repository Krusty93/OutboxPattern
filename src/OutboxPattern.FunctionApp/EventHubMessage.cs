using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OutboxPattern.FunctionApp;

public record Column(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("jdbcType")] int JdbcType,
    [property: JsonPropertyName("nativeType")] object NativeType,
    [property: JsonPropertyName("typeName")] string TypeName,
    [property: JsonPropertyName("typeExpression")] string TypeExpression,
    [property: JsonPropertyName("charsetName")] object CharsetName,
    [property: JsonPropertyName("length")] object Length,
    [property: JsonPropertyName("scale")] int? Scale,
    [property: JsonPropertyName("position")] int Position,
    [property: JsonPropertyName("optional")] bool Optional,
    [property: JsonPropertyName("autoIncremented")] bool AutoIncremented,
    [property: JsonPropertyName("generated")] bool Generated
);

public record Root(
    [property: JsonPropertyName("source")] Source Source,
    [property: JsonPropertyName("databaseName")] string DatabaseName,
    [property: JsonPropertyName("schemaName")] string SchemaName,
    [property: JsonPropertyName("ddl")] object Ddl,
    [property: JsonPropertyName("tableChanges")] IReadOnlyList<TableChange> TableChanges
);

public record Source(
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("connector")] string Connector,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("ts_ms")] long TsMs,
    [property: JsonPropertyName("snapshot")] string Snapshot,
    [property: JsonPropertyName("db")] string Db,
    [property: JsonPropertyName("sequence")] object Sequence,
    [property: JsonPropertyName("schema")] string Schema,
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("change_lsn")] object ChangeLsn,
    [property: JsonPropertyName("commit_lsn")] string CommitLsn,
    [property: JsonPropertyName("event_serial_no")] object EventSerialNo
);

public record SqlTable(
    [property: JsonPropertyName("defaultCharsetName")] object DefaultCharsetName,
    [property: JsonPropertyName("primaryKeyColumnNames")] IReadOnlyList<string> PrimaryKeyColumnNames,
    [property: JsonPropertyName("columns")] IReadOnlyList<Column> Columns
);

public record TableChange(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("table")] SqlTable Table
);

public record OutboxMessage(
    [property: JsonPropertyName("op")] string Operation,
    [property: JsonPropertyName("source")] Source Source
);
