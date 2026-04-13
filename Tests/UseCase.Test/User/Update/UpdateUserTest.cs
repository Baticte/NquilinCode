using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Services.LoggedUser;
using CommonTestUtilities.Validators;
using NquilinCode.Application.UseCases.User.Update;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;
using Shouldly;
using Email = NquilinCode.Domain.ValueObjects.Email;

namespace UseCase.Test.User.Update;

public class UpdateUserTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request, CancellationToken.None); };

        await act.ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.Value.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request, CancellationToken.None); };

        var exception = await act.ShouldThrowAsync<RegisterUserValidationException>();
        exception.ErrorMessages.ShouldContain(ValidationMessages.NAME_REQUIRED);
    }
    
    [Fact]
    public async Task Fail_When_Empty_Email()
    {
        var (user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;
        
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request, CancellationToken.None);

        var exception = await act.ShouldThrowAsync<RegisterUserValidationException>();
        
        exception.ErrorMessages.Count.ShouldBe(1);
        
        var message = exception.ErrorMessages[0];
        message.ShouldBe(ValidationMessages.EMAIL_REQUIRED);
    }
    
    [Fact]
    public async Task Fail_When_Invalid_Email()
    {
        var (user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "invalid_email";
        
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request, CancellationToken.None);

        var exception = await act.ShouldThrowAsync<RegisterUserValidationException>();
        
        exception.ErrorMessages.Count.ShouldBe(1);
        exception.ErrorMessages.Contains(ValidationMessages.INVALID_EMAIL_FORMAT).ShouldBeTrue();
    }
    
    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var (user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
            
        var email = new Email(request.Email);

        var useCase = CreateUseCase(user, email.Value);

        var act = async () => await useCase.Execute(request, CancellationToken.None);

        var singleError = (await act.ShouldThrowAsync<RegisterUserValidationException>()).ErrorMessages.ShouldHaveSingleItem();

        singleError.ShouldBe(ValidationMessages.EXISTS_USER);
    }
    
    private static UpdateUser CreateUseCase(NquilinCode.Domain.Entities.User user, string? email = null)
    {
        var validator =  ValidatorBuilder.Build<UpdateUserValidator, RequestUpdateUserJson>();
        var loggedUser = LoggedUserBuilder.Build(user);
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var userReadRepository = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        Email? currentEmail = null;
        if (string.IsNullOrEmpty(email))
            return new UpdateUser(validator, loggedUser, userUpdateRepository, userReadRepository.Build(), unitOfWork);
            
        currentEmail = new Email(email);
        userReadRepository.ExistActiveUserWithEmail(currentEmail);
        
        return new UpdateUser(validator, loggedUser, userUpdateRepository, userReadRepository.Build(), unitOfWork);
    }
}