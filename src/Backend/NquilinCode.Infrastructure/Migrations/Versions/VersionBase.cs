using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace NquilinCode.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTableBase(string tableName)
    {
        return Create.Table(tableName)
            .WithColumn("Id").AsGuid().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedAt")
            .AsCustom("timestamp with time zone").NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("DeletedAt").AsCustom("timestamp with time zone").Nullable()
            .WithColumn("DeletedBy").AsGuid().Nullable();
    }
}