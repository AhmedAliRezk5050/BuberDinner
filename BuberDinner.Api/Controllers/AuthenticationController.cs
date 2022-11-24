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
        ErrorOr.ErrorOr<AuthenticationResult> authResult =
            _authenticationService.Register(
                      result.FirstName,
                      result.LastName,
                      result.Email,
                      result.Password);
        return authResult.MatchFirst(
            a => Ok(MapAuthResult(a)),
            firstError => Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: firstError.Description)
            );
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
