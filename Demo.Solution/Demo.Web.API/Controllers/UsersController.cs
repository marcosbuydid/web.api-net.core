using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo.Web.API.DatabaseContext;
using Demo.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Demo.Web.API.Interfaces;

namespace Demo.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public UsersController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User userToCreate)
        {
            //[Authorize(Roles = "Administrator")]
            //if the user is not administrator return automatically
            //forbidden result and not enters to the endpoint.
            //is a better solution and for custom messages middlewares are used.

            //Authorize
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validatedToken = _authService.ValidateToken(token);

            //if user is autenticated
            if (validatedToken != null)
            {
                //get user role
                var role = _authService.GetUserRole(validatedToken);

                if (role == "Administrator")
                {
                    var user = _context.Users.Where(user => user.Email == userToCreate.Email).FirstOrDefault();
                    if (user != null)
                    {
                        return new JsonResult(new { error = "Cannot create user, email is already registered" }) { StatusCode = StatusCodes.Status202Accepted };
                    }

                    _context.Users.Add(userToCreate);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetUser", new { id = userToCreate.Id }, userToCreate);
                }
                else
                {
                    return new JsonResult(new { authorization = "Only Administrators can create users" }) { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
            //return StatusCode(401, "Authorization required");
            return new JsonResult(new { authorization = "Authorization required" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
