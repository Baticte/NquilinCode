using NquilinCode.Communication.Requests;

namespace NquilinCode.Application.Abstractions.UseCases.User;

public interface IUpdateUser
{
    public Task Execute(RequestUpdateUserJson request, CancellationToken cancellationToken);
}