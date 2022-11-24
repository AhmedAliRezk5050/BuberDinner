using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneOf;

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
        OneOf<AuthenticationResult, DuplicateEmailError> registerResult =
            _authenticationService.Register(
                      result.FirstName,
                      result.LastName,
                      result.Email,
                      result.Password);
        if (registerResult.IsT0)
        {
            var authResult = registerResult.AsT0;
            AuthenticationResponse response = MapAuthResult(authResult);
            return Ok(response);
        }


        return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");
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
