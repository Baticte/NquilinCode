using FluentValidation;
using Mapster;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;
using NquilinCode.Domain.Repositories;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Domain.ValueObjects;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;
using NquilinCode.Kernel.Validators;

namespace NquilinCode.Application.UseCases.User.Register;

public class RegisterUser : IRegisterUser
{
    private readonly IValidator<RequestRegisterUserJson> _validator;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUser(IValidator<RequestRegisterUserJson> validator, IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository, IAccessTokenGenerator accessTokenGenerator,
        IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request,
        CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);

        var email = new Email(request.Email);
        var password = new Password(request.Password);
        
        var passwordHash = _passwordHasher.HashPassword(password);

        var user = Domain.Entities.User.Create(request.Name, email, new Password(passwordHash));
        
        await _writeOnlyRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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

    private async Task Validate(RequestRegisterUserJson request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        var result = ValidationResult.Success();

        if (!validationResult.IsValid)
        {
            result.Merge(ValidationResult.Failure(validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var email = new Email(request.Email);

        var emailExists = await _readOnlyRepository.ExistActiveUserWithEmailAsync(email, cancellationToken);
        if (emailExists)
            result.Merge(ValidationResult.Failure(ValidationMessages.EXISTS_USER));

        if (!result.IsSuccess)
        {
            throw new RegisterUserValidationException(result.Errors);
        }
    }
}