using Microsoft.AspNetCore.Mvc;
using NquilinCode.API.Attributes;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;

namespace NquilinCode.API.Controllers;

public class UserController : EntityBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterUser useCase,
        [FromBody] RequestRegisterUserJson request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticateUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(cancellationToken);

        return Ok(result);
    }
}