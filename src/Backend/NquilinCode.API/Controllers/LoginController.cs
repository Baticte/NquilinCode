using Microsoft.AspNetCore.Mvc;
using NquilinCode.Application.Abstractions.UseCases.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Communication.Responses;

namespace NquilinCode.API.Controllers;

public class LoginController : EntityBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILogin useCase,
        [FromBody] RequestLoginJson request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(request, cancellationToken);
    
        return Ok(result);
    }
}