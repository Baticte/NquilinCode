using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;

namespace NquilinCode.Application.Abstractions.UseCases.User;

public interface ILogin
{
    public Task<ResponseRegisterUserJson> Execute(RequestLoginJson request, CancellationToken cancellationToken);
}