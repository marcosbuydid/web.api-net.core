using Demo.Web.API.Models;

namespace Demo.Web.API.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
