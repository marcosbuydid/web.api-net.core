using Microsoft.AspNetCore.Mvc;
using Demo.Controllers.Models;

namespace Demo.Controllers.ControllersExample
{
    public class RegisterController : Controller
    {

        [Route("/api/register")]

        public IActionResult Index(Person person)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n",
                    ModelState.Values.SelectMany(
                        value => value.Errors).Select(
                        e => e.ErrorMessage));
                return BadRequest(errors);
            }

            return new JsonResult(person);
        }
    }
}
