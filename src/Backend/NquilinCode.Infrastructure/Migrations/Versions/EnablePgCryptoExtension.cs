using FluentMigrator;

namespace NquilinCode.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.ENABLE_PgCrypto_EXTENSION, "Enables the required PostgresSQL pgcrypto extension")]
public class EnablePgCryptoExtension : Migration
{
    public override void Up()
    {
        Execute.Sql(@"CREATE EXTENSION IF NOT EXISTS ""pgcrypto"";");
    }

    public override void Down()
    {
    }
}