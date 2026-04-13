using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NquilinCode.Domain.ValueObjects;
using NquilinCode.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private NquilinCode.Domain.Entities.User _user = default!;
    private string _password = string.Empty;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NquilinCodeDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<NquilinCodeDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<NquilinCodeDbContext>();

                dbContext.Database.EnsureCreated();

                StartDatabase(dbContext);
            });
    }

    public Guid GetId() => _user.Id;
    public Email GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    
    private void StartDatabase(NquilinCodeDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();
        
        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
}