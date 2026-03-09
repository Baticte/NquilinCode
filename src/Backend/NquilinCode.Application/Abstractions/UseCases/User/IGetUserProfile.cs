using NquilinCode.Communication.Responses;

namespace NquilinCode.Application.Abstractions.UseCases.User;

public interface IGetUserProfile
{
    public Task<ResponseUserProfileJson> Execute(CancellationToken cancellationToken);
}