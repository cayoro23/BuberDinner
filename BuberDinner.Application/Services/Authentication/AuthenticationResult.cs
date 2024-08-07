namespace BuberDinner.Application.Services.Authentication;

public record AuthenticationResult(
    Guid Id,
    string FirsName,
    string LastName,
    string Email,
    string Token);
