using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Application.Services.Cryptography;
using NquilinCode.Application.UseCases.User.Login.DoLogin;
using NquilinCode.Application.UseCases.User.Profile;
using NquilinCode.Application.UseCases.User.Register;

namespace NquilinCode.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        AddUseCases(services);
        AddPasswordHasher(services);
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUser, RegisterUser>();
        services.AddScoped<ILogin, Login>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
    }

    private static void AddPasswordHasher(IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}