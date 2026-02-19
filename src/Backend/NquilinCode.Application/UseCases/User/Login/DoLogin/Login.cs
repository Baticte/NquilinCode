using FluentValidation;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Exceptions.BaseException;

namespace NquilinCode.Application.UseCases.User.Login.DoLogin;

public class Login : ILogin
{
    private readonly IValidator<RequestLoginJson> _validator;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;

    public Login(IValidator<RequestLoginJson> validator, IUserReadOnlyRepository readOnlyRepository, IPasswordHasher passwordHasher)
    {
        _validator = validator;
        _readOnlyRepository = readOnlyRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request, CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);

        var user = await _readOnlyRepository.GetByEmailAsync(request.Email, cancellationToken) ??
                   throw new InvalidLoginException();

        var result = _passwordHasher.VerifyPassword(request.Password, user.Password);
        if (!result) throw new InvalidLoginException();
        
        return new ResponseRegisterUserJson
        {
            Name = user.Name
        };
    }

    private async Task Validate(RequestLoginJson request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new RegisterUserValidationException(errorMessages);
        }
    }
}