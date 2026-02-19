using CommonTestUtilities.Requests.User;
using NquilinCode.Application.UseCases.User.Login.DoLogin;
using NquilinCode.Communication.Requests;
using Shouldly;

namespace Validators.Test.User.Login.DoLogin;

public class LoginValidatorTest
{
    [Fact]
    public void Success()
    {
        var (validator, request) = CreateValidatorAndRequest();

        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
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
    
    private static (LoginValidator validator, RequestLoginJson request) CreateValidatorAndRequest(int passwordLength = 10)
    {
        return (
            new LoginValidator(),
            RequestLoginJsonBuilder.Build(passwordLength)
        );
    }
}