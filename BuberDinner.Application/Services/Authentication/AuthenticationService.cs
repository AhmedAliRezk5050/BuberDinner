using BuberDinner.Application.Common.Interfaces.Authentication;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
  private readonly IJwtTokenGenerator _jwtTokenGenerator;

  public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
  }

  public AuthenticationResult Login(string email, string password)
  {
    return new AuthenticationResult(Guid.NewGuid(), "ahmed", "ali", email, "token");
  }

  public AuthenticationResult Register(string firstName, string lastName, string email, string password)
  {

    Guid id = Guid.NewGuid();

    var token = _jwtTokenGenerator.GenerateToken(id, firstName, lastName);

    return new AuthenticationResult(id, firstName, lastName, email, token);
  }
}