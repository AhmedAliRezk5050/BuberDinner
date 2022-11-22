using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Login(string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("User with given email does not exist.");
        }

        if (user.Password != password)
        {
            throw new Exception("Invalid password");
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, firstName: user.FirstName, lastName: user.LastName);

        return new AuthenticationResult(user.Id, user.FirstName, user.LastName, email, token);
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("User with given email already exists");
        }

        var user = new User()
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, firstName, lastName);

        return new AuthenticationResult(user.Id, firstName, lastName, email, token);
    }
}