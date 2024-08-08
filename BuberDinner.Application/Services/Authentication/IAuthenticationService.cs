using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    ErrorOr<AuthenticationResult> Register(string firsName, string lastName, string email, string password);
    ErrorOr<AuthenticationResult> Login(string email, string password);
}
