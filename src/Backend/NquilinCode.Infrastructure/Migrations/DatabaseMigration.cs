using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NquilinCode.Domain.Enums;

namespace NquilinCode.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
    {
        switch (databaseType)
        {
            case DatabaseType.InMemory:
                return;
            case DatabaseType.Postgres:
                EnsureDatabaseCreated_Postgres(connectionString);
                break;
            case DatabaseType.SqlServer:
                EnsureDatabaseCreated_SqlServer(connectionString);
                break;
            default:
                throw new InvalidOperationException("Database type not supported");
        }

        MigrationDatabase(serviceProvider);
    }

    private static void EnsureDatabaseCreated_Postgres(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var databaseName = builder.Database;

        builder.Database = "postgres";

        using var dbConnection = new NpgsqlConnection(builder.ConnectionString);
        dbConnection.Open();

        var exists = dbConnection.ExecuteScalar<bool>(
            "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @name);",
            new { name = databaseName }
        );

        if (!exists)
        {
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
        }
    }
    
    private static void EnsureDatabaseCreated_SqlServer(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;

        builder.InitialCatalog = "master";

        using var connection = new SqlConnection(builder.ConnectionString);
        connection.Open();

        var exists = connection.ExecuteScalar<bool>(
            "SELECT 1 FROM sys.databases WHERE name = @name",
            new { name = databaseName }
        );

        if (!exists)
        {
            connection.Execute($"CREATE DATABASE {databaseName}");
        }
    }


    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}