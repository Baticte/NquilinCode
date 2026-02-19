using NquilinCode.API.Converters;
using NquilinCode.API.Filters;
using NquilinCode.Application;
using NquilinCode.Application.Services.Mapster;
using NquilinCode.Infrastructure;
using NquilinCode.Infrastructure.Extensions;
using NquilinCode.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); })
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new StringConverter()); });

builder.Services.AddInfrastructureServices(builder.Configuration); // regista DbContext + Repositórios
builder.Services.AddApplicationServices(); // regista serviços da camada Application

MapsterConfiguration.Configure();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

MigrateDatabase();

app.UseMiddleware<CultureMiddleware>();

app.MapControllers();

await app.RunAsync();
return;

void MigrateDatabase()
{
    var databaseType = builder.Configuration.DatabaseType();
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
}