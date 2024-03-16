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
        private readonly IAuthService _authService;
        public SessionController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] UserDTO userDTO)
        {
            var user = _authService.ValidateUser(userDTO.Email, userDTO.Password);

            if (user == null)
            {
                return BadRequest("User or password is incorrect. Try again");
            }
            else
            {
                var jwtToken = _authService.GenerateToken(user);
                return new JsonResult(new { token = jwtToken }) { StatusCode = StatusCodes.Status200OK };
            }
        }

        
        [HttpPost]
        [Route("refreshtoken")]
        public ActionResult RefreshToken()
        {
            //Authorize
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validatedToken = _authService.ValidateToken(token);

            //if user is autenticated
            if (validatedToken != null)
            {
                var refreshToken = _authService.RefreshToken(validatedToken);
                return new JsonResult(new { token = refreshToken }) { StatusCode = StatusCodes.Status200OK };
            }
            return new JsonResult(new { authorization = "Authorization required" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

    }
}
