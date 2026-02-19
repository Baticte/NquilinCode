using Microsoft.Extensions.Configuration;
using NquilinCode.Domain.Enums;

namespace NquilinCode.Infrastructure.Extensions;

public static class ConfigurationExtension
{
        extension(IConfiguration configuration)
        {
            public DatabaseType DatabaseType()
            {
                var databaseType = configuration["Database:Type"];

                return Enum.Parse<DatabaseType>(databaseType!);
            }

            public string ConnectionString()
            {
                var databaseType = configuration.DatabaseType();

                return databaseType switch
                {
                    Domain.Enums.DatabaseType.Postgres => configuration.GetConnectionString("Postgres")!,
                    Domain.Enums.DatabaseType.MySql => configuration.GetConnectionString("MySql")!,
                    _ => configuration.GetConnectionString("SqlServer")!
                };
            }
        }
}