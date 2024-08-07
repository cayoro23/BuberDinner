using BuberDinner.Application.Common.Interfaces.Authentication;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwTokenGenerator _jwtGenerator;

    public AuthenticationService(IJwTokenGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    public AuthenticationResult Register(string firsName, string lastName, string email, string password)
    {
        // Check if user already exists

        // Create user (generate unique ID)

        // Create JWT token

        Guid userId = Guid.NewGuid();

        var token = _jwtGenerator.GenerateToken(userId, firsName, lastName);

        return new AuthenticationResult(
            userId,
            firsName,
            lastName,
            email,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(
            Guid.NewGuid(),
            "firsName",
            "lastName",
            email,
            "token");
    }
}
