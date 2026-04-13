using FluentValidation;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Domain.ValueObjects;
using NquilinCode.Exceptions.BaseException;

namespace NquilinCode.Application.UseCases.User.Login.DoLogin;

public class Login : ILogin
{
    private readonly IValidator<RequestLoginJson> _validator;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public Login(IValidator<RequestLoginJson> validator, IUserReadOnlyRepository readOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator, IPasswordHasher passwordHasher)
    {
        _validator = validator;
        _readOnlyRepository = readOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request, CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);

        var email = new Email(request.Email);

        var user = await _readOnlyRepository.GetByEmailAsync(email, cancellationToken) ??
                   throw new InvalidLoginException();

        var password = new Password(request.Password);

        var result = _passwordHasher.VerifyPassword(password, user.Password);
        if (!result) throw new InvalidLoginException();

        var tokenResult = _accessTokenGenerator.GenerateAccessToken(user.Id);

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = tokenResult.Token,
                AccessTokenExpiration = tokenResult.ExpirationDate
            }
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