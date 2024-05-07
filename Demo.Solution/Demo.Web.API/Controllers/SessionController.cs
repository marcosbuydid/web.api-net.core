using Demo.Web.API.DatabaseContext;
using Demo.Web.API.Filters;
using Demo.Web.API.Interfaces;
using Demo.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Demo.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        public SessionController(IAuthService authService, ApplicationDbContext context)
        {
            _context = context;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Session>> Login([FromBody] UserDTO userDTO)
        {
            var user = _authService.ValidateUser(userDTO.Email, userDTO.Password);

            if (user == null)
            {
                return BadRequest("User or password is incorrect. Try again");
            }
            else
            {
                var jwtToken = _authService.GenerateToken(user);

                //every session is asociated with a valid token, no matter if a user has unexpired tokens.
                //every time at log in a new session is created with a different token.
                //no logout is required because session duration is the expiration time of the token.

                Session session = new Session();
                session.Token = jwtToken;
                session.Email = userDTO.Email;
                session.CreatedAt = DateTime.UtcNow;

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();

                return new JsonResult(new { token = jwtToken })
                { StatusCode = StatusCodes.Status200OK };
            }
        }

        [AuthorizationFilter("Administrator,User")]
        [HttpPost]
        [Route("refreshtoken")]
        public ActionResult RefreshToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validatedToken = _authService.ValidateToken(token);
            var refreshToken = _authService.RefreshToken(validatedToken);
            return new JsonResult(new { token = refreshToken }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
