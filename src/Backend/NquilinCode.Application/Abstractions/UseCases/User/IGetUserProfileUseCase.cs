using NquilinCode.Communication.Responses;

namespace NquilinCode.Application.Abstractions.UseCases.User;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute(CancellationToken cancellationToken);
}