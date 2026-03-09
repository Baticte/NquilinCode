using FluentValidation;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Domain.Repositories;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.Services.LoggedUser;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Application.UseCases.User.Update;

public class UpdateUser : IUpdateUser
{
    private readonly IValidator<RequestUpdateUserJson> _validator;
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUser(IValidator<RequestUpdateUserJson> validator, ILoggedUser loggedUser,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.User(cancellationToken);

        await Validate(request, loggedUser.Email, cancellationToken);

        var user = await _updateOnlyRepository.GetByIdAsync(loggedUser.Id, cancellationToken);

        if(user == null) return;

        user.Name = request.Name;
        user.Email = request.Email;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);

        if (!string.IsNullOrEmpty(request.Email) && !currentEmail.Equals(request.Email))
        {
            var userExist = await _readOnlyRepository.ExistActiveUserWithEmailAsync(request.Email, cancellationToken);
            if(userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ValidationMessages.EXISTS_USER));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new RegisterUserValidationException(errorMessages);
        }
    }
}