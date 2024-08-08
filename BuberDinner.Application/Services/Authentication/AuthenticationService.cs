using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwTokenGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwTokenGenerator jwtGenerator, IUserRepository userRepository)
    {
        _jwtGenerator = jwtGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Register(string firsName, string lastName, string email, string password)
    {
        // 1. Validamos si el usuario existe
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("El usuario con el correo ya existe");
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

        var token = _jwtGenerator.GenerateToken(user.Id, firsName, lastName);

        return new AuthenticationResult(
            user.Id,
            firsName,
            lastName,
            email,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        // 1. Validar si el usuario existe
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("No existe el usuario con ese correo");
        }

        // 2. Validar si la contraseña es correcta
        if (user.Password != password)
        {
            throw new Exception("Contraseña incorrecta");
        }

        // 3. Creamos un JWT Token
        var token = _jwtGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new AuthenticationResult(
            Guid.NewGuid(),
            user.FirstName,
            user.LastName,
            email,
            token);
    }
}
