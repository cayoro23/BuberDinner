using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication.Commands;

public class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IJwTokenGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationCommandService(IJwTokenGenerator jwtGenerator, IUserRepository userRepository)
    {
        _jwtGenerator = jwtGenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Register(string firsName, string lastName, string email, string password)
    {
        // 1. Validamos si el usuario existe
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Creamos usuario (Generando ID Unico) & Persistimos en la BD
        var user = new User
        {
            FirstName = firsName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        // 3. Creamos un JWT Token

        var token = _jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
