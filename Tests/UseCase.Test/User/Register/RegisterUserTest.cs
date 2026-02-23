using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using CommonTestUtilities.Validators;
using NquilinCode.Application.UseCases.User.Register;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;
using Shouldly;

namespace UseCase.Test.User.Register;

public class RegisterUserTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request, CancellationToken.None);

        var singleError = (await act.ShouldThrowAsync<RegisterUserValidationException>())
            .ErrorMessages.ShouldHaveSingleItem();

        singleError.ShouldBe(ValidationMessages.EXISTS_USER);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request, CancellationToken.None);

        var singleError = (await act.ShouldThrowAsync<RegisterUserValidationException>())
            .ErrorMessages.ShouldHaveSingleItem();
        singleError.ShouldBe(ValidationMessages.NAME_REQUIRED);
    }

    private static RegisterUser CreateUseCase(string? email = null)
    {
        var validator = ValidatorBuilder.Build<RegisterUserValidator, RequestRegisterUserJson>();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var jwtAccessTokenBuilder = JwtAccessTokenGeneratorBuilder.Build();
        var passwordHasher = PasswordHasherBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (!string.IsNullOrEmpty(email))
        {
            readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);
        }

        return new RegisterUser(validator, writeOnlyRepository, readOnlyRepositoryBuilder.Build(),
            jwtAccessTokenBuilder, passwordHasher, unitOfWork);
    }
}