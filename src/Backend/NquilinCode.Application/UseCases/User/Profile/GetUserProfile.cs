using Mapster;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Responses;
using NquilinCode.Domain.Services.LoggedUser;

namespace NquilinCode.Application.UseCases.User.Profile;

public class GetUserProfile : IGetUserProfile
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfile(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseUserProfileJson> Execute(CancellationToken cancellationToken)
    {
        var user = await _loggedUser.User(cancellationToken);

        return user.Adapt<ResponseUserProfileJson>();
    }
}