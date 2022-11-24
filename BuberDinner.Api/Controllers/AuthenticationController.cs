using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;


[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest result)
    {
        FluentResults.Result<AuthenticationResult> registerResult =
            _authenticationService.Register(
                      result.FirstName,
                      result.LastName,
                      result.Email,
                      result.Password);
        if (registerResult.IsSuccess)
        {
            return Ok(MapAuthResult(registerResult.Value));
        }

        var firstError = registerResult.Errors[0];

        if(firstError is DuplicateEmailError)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, detail: "Email already exists.");
        }

        return Problem();
    }


    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        var authResult = _authenticationService.Login(
          loginRequest.Email,
          loginRequest.Password);


        var response = new AuthenticationResponse(
         authResult.User.Id,
         authResult.User.FirstName,
         authResult.User.LastName,
         authResult.User.Email,
         authResult.Token
       );
        return Ok(response);
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
                      authResult.User.Id,
                      authResult.User.FirstName,
                      authResult.User.LastName,
                      authResult.User.Email,
                      authResult.Token
                    );
    }

}
