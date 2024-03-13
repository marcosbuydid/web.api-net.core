using Demo.Web.API.DatabaseContext;
using Demo.Web.API.Interfaces;
using Demo.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Demo.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        public LoginController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] UserDTO userDTO)
        {
            var user = _context.Users.Where(user => user.Email == userDTO.Email && user.Password == userDTO.Password).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("User or password is incorrect. Try again");
            }
            else
            {
                var token = _authService.GenerateToken(user);
                //return Ok(token);
                return new JsonResult(token);
            }
        }

    }
}
