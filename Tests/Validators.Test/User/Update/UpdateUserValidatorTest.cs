using CommonTestUtilities.Requests.User;
using NquilinCode.Application.UseCases.User.Update;
using NquilinCode.Communication.Requests;
using Shouldly;

namespace Validators.Test.User.Update;

public class UpdateUserValidatorTest
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

    private static (UpdateUserValidator validator, RequestUpdateUserJson request) CreateValidatorAndRequest()
    {
        return (
            new UpdateUserValidator(),
            RequestUpdateUserJsonBuilder.Build());
    }
}