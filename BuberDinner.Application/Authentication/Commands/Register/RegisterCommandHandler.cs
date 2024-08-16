using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwTokenGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwTokenGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // 1. Validamos si el usuario existe
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Creamos usuario (Generando ID Unico) & Persistimos en la BD
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password
        };

        _userRepository.Add(user);

        // 3. Creamos un JWT Token

        var token = _jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
