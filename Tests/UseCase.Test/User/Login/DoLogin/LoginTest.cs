using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using CommonTestUtilities.Validators;
using NquilinCode.Application.UseCases.User.Login.DoLogin;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;
using Shouldly;

namespace UseCase.Test.User.Login.DoLogin;

public class LoginTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user, password);

        var result = await useCase.Execute(
            new RequestLoginJson { Email = user.Email.Value, Password = password }, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request, CancellationToken.None);

        var exception = await act.ShouldThrowAsync<InvalidLoginException>();
        exception.Message.ShouldBe(ValidationMessages.INVALID_EMAIL_OR_PASSWORD);
    }

    private static NquilinCode.Application.UseCases.User.Login.DoLogin.Login CreateUseCase(
        NquilinCode.Domain.Entities.User? user = null, string password = default!)
    {
        var validator = ValidatorBuilder.Build<LoginValidator, RequestLoginJson>();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var jwtAccessTokenBuilder = JwtAccessTokenGeneratorBuilder.Build();
        var passwordEncripter = PasswordHasherBuilder.Build();

        if (user is not null)
        {
            readRepositoryBuilder.GetByEmailAsync(user);
        }

        return new NquilinCode.Application.UseCases.User.Login.DoLogin.Login(validator, readRepositoryBuilder.Build(),
            jwtAccessTokenBuilder, passwordEncripter);
    }
}