using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Application.Services.Authentication.Queries;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;


[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationCommandService _authenticationCommandService;
    private readonly IAuthenticationQueryService _authenticationQueryService;

    public AuthenticationController(
        IAuthenticationCommandService authenticationCommandService,
        IAuthenticationQueryService authenticationQueryService)
    {
        _authenticationCommandService = authenticationCommandService;
        _authenticationQueryService = authenticationQueryService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest result)
    {
        ErrorOr.ErrorOr<AuthenticationResult> authResult =
            _authenticationCommandService.Register(
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
        ErrorOr.ErrorOr<AuthenticationResult> authResult = _authenticationQueryService.Login(
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
