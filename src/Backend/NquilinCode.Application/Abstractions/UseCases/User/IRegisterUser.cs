using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;

namespace NquilinCode.Application.Abstractions.UseCases.User;

public interface IRegisterUser
{
    public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request, CancellationToken cancellationToken);
}