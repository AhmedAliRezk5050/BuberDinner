using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;


[Route("auth")]
public class AuthenticationController : ApiController
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
        return authResult.Match(
            a => Ok(MapAuthResult(a)),
            errors => Problem(errors)
            );
    }


    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        ErrorOr.ErrorOr<AuthenticationResult> authResult = _authenticationService.Login(
          loginRequest.Email,
          loginRequest.Password);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized
                , title: authResult.FirstError.Description);
        }

        return authResult.Match(
                a => Ok(MapAuthResult(a)),
                errors => Problem(errors)
            );
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
