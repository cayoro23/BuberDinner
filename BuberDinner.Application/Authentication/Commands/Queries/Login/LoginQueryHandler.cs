using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Queries.Login;

public class LoginQueryHandler: 
    IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwTokenGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IJwTokenGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // 1. Validar si el usuario existe
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validar si la contraseña es correcta
        if (user.Password != query.Password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 3. Creamos un JWT Token
        var token = _jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
