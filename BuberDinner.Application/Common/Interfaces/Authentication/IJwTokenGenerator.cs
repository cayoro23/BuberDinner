using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Authentication;

public interface IJwTokenGenerator
{
    string GenerateToken(User user);
}
