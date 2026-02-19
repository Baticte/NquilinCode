using FluentMigrator;

namespace NquilinCode.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER, "Create table to save the user information")]
public class Version000001 : VersionBase
{
    public override void Up()
    {
        CreateTableBase("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable().Unique()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}