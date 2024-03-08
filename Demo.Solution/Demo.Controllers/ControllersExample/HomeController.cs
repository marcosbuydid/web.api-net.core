using Microsoft.AspNetCore.Mvc;
using Demo.Controllers.Models;

namespace Demo.Controllers.ControllersExample
{
    public class HomeController
    {

        [Route("/test/controller")]
        public string method1()
        {
            return "output of method1";
        }

        //after app started add on the url /test/controller 
        //and the return of method1 should be seen in the 
        //web explorer


        [Route("/test/person")]

        public JsonResult Person()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Aaron",
                LastName = "Williams",
                Age = 29
            };
            return new JsonResult(person);
            
        }
    }

    [Controller]
    public class Contact
    {
        [Route("/test/contact")]
        public string method2()
        {
            return "output of method2";
        }

        //if you use [Controller] or the name of the class
        //include Controller is the same to define a controller.
    }
}
