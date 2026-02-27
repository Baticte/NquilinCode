using Microsoft.OpenApi;
using NquilinCode.API.Converters;
using NquilinCode.API.Filters;
using NquilinCode.API.Token;
using NquilinCode.Application;
using NquilinCode.Application.Services.Mapster;
using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Infrastructure;
using NquilinCode.Infrastructure.Extensions;
using NquilinCode.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, // 'Http' is now heavily preferred over 'ApiKey' for JWTs
        Scheme = "bearer",              
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token (you no longer need to type 'Bearer ' first).\n\nExample: \"12345eyJhbGci...\""
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

builder.Services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); })
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new StringConverter()); });

builder.Services.AddInfrastructureServices(builder.Configuration); // regista DbContext + Repositórios
builder.Services.AddApplicationServices(); // regista serviços da camada Application

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

MapsterConfiguration.Configure();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

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