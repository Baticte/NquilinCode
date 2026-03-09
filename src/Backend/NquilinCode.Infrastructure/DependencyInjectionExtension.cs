using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NquilinCode.Domain.Enums;
using NquilinCode.Domain.Repositories;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Domain.Services.LoggedUser;
using NquilinCode.Infrastructure.DataAccess;
using NquilinCode.Infrastructure.DataAccess.Repositories;
using NquilinCode.Infrastructure.Extensions;
using NquilinCode.Infrastructure.Security.Tokens.Access.Generator;
using NquilinCode.Infrastructure.Security.Tokens.Access.Validator;
using NquilinCode.Infrastructure.Services.LoggedUser;

namespace NquilinCode.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseTypeEnum = configuration.DatabaseType();
        
        if (databaseTypeEnum == DatabaseType.SqlServer)
        {
            AddDbContextSqlServer(services, configuration);
            AddFluentMigratorSqlServer(services, configuration);
        }
        else if (databaseTypeEnum == DatabaseType.Postgres)
        {
            AddDbContextPostgres(services, configuration);
            AddFluentMigratorPostgres(services, configuration);
        }
        
        AddRepositories(services);

        AddTokens(services, configuration);
        AddLoggedUser(services);
    }

    private static void AddDbContextSqlServer(IServiceCollection services, IConfiguration configuration)
    { 
        var connectionString = configuration.ConnectionString();
        
        services.AddDbContext<NquilinCodeDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    private static void AddDbContextPostgres(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddDbContext<NquilinCodeDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddFluentMigratorSqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer2016()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("NquilinCode.Infrastructure")).For.All();
        });
    }
    
    private static void AddFluentMigratorPostgres(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddPostgres15_0()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("NquilinCode.Infrastructure")).For.All();
        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");

        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(_ =>
            new JwtAccessTokenGenerator(expirationTimeMinutes, signingKey!));

        services.AddScoped<IAccessTokenValidator>(option =>
            new JwtAccessTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
}