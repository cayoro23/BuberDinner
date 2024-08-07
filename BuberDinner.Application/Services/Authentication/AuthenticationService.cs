﻿using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

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

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        // 1. Validar si el usuario existe
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validar si la contraseña es correcta
        if (user.Password != password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 3. Creamos un JWT Token
        var token = _jwtGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
