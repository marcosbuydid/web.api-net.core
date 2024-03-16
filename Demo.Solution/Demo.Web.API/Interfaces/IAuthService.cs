using Demo.Web.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Demo.Web.API.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        User ValidateUser(string userEmail, string userPassword);
        SecurityToken ValidateToken(string token);
        string GetUserRole(SecurityToken validatedToken);
        string RefreshToken(SecurityToken validatedToken);
    }
}
