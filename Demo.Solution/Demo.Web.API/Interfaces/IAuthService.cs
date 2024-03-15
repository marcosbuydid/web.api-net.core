using Demo.Web.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Web.API.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        User ValidateUser(string userEmail, string userPassword);
        SecurityToken ValidateToken(string token);
        string GetUserRole(SecurityToken validatedToken);
    }
}
