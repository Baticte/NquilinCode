using CommonTestUtilities.Requests.User;
using NquilinCode.Application.UseCases.User.Register;
using NquilinCode.Communication.Requests;
using Shouldly;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var (validator, request) = CreateValidatorAndRequest();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Fail_When_Name_Empty()
    {
        var (validator, request) = CreateValidatorAndRequest();
        
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.Single();
        error.PropertyName.ShouldBe("Name");
    }

    [Fact]
    public void Fail_When_Email_Empty()
    {
        var (validator, request) = CreateValidatorAndRequest();
        
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.Single();
        error.PropertyName.ShouldBe("Email");
    }

    [Fact]
    public void Fail_When_Email_Invalid_Format()
    {
        var (validator, request) = CreateValidatorAndRequest();
        
        request.Email = "invalidEmail";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.Single();
        error.PropertyName.ShouldBe("Email");
    }

    [Fact]
    public void Fail_When_Password_Empty()
    {
        var (validator, request) = CreateValidatorAndRequest();

        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.Single();
        error.PropertyName.ShouldBe("Password");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Fail_WhenPassword_Invalid_MinimumLength(int passwordLength)
    {
        var (validator, request) = CreateValidatorAndRequest(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.Single();
        error.PropertyName.ShouldBe("Password");
    }

    private static (RegisterUserValidator validator, RequestRegisterUserJson request) CreateValidatorAndRequest(int passwordLength = 10)
    {
        return (
            new RegisterUserValidator(),
            RequestRegisterUserJsonBuilder.Build(passwordLength)
        );
    }
}