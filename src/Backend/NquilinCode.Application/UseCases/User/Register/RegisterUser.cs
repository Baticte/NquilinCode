using FluentValidation;
using Mapster;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;
using NquilinCode.Domain.Repositories;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;
using NquilinCode.Kernel.Validators;

namespace NquilinCode.Application.UseCases.User.Register;

public class RegisterUser : IRegisterUser
{
    private readonly IValidator<RequestRegisterUserJson> _validator;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUser(IValidator<RequestRegisterUserJson> validator, IUserWriteOnlyRepository writeOnlyRepository, IUserReadOnlyRepository readOnlyRepository,
        IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request,
        CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);

        var user = request.Adapt<Domain.Entities.User>();
        user.Password = _passwordHasher.HashPassword(request.Password);

        await _writeOnlyRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResponseRegisterUserJson
        {
            Name = user.Name
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

        var emailExists = await _readOnlyRepository.ExistActiveUserWithEmailAsync(request.Email, cancellationToken);
        if (emailExists)
            result.Merge(ValidationResult.Failure(ValidationMessages.EXISTS_USER));

        if (!result.IsSuccess)
        {
            throw new RegisterUserValidationException(result.Errors);
        }
    }
}