using Mapster;
using NquilinCode.Communication.Requests;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Application.Services.Mapster;

public static class MapsterConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig()
            .Ignore(dest => dest.Password);
        
        TypeAdapterConfig<Email, string>.NewConfig()
            .MapWith(src => src.Value);
    }
}