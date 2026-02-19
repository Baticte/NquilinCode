using Mapster;
using NquilinCode.Communication.Requests;
using NquilinCode.Domain.Entities;

namespace NquilinCode.Application.Services.Mapster;

public static class MapsterConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig()
            .Ignore(dest => dest.Password);
    }
}