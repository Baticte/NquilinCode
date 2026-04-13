using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Application.UseCases.User.Login.DoLogin;
using NquilinCode.Application.UseCases.User.Profile;
using NquilinCode.Application.UseCases.User.Register;
using NquilinCode.Application.UseCases.User.Update;

namespace NquilinCode.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        AddUseCases(services);
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUser, RegisterUser>();
        services.AddScoped<ILogin, Login>();
        services.AddScoped<IGetUserProfile, GetUserProfile>();
        services.AddScoped<IUpdateUser, UpdateUser>();
    }
}