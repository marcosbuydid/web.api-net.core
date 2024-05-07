using Demo.Web.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Web.API.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private List<string> _role;
        public AuthorizationFilter(string role = "")
        {
            _role = role.Split(",").ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionLogic = this.GetAuthService(context);

            //If token is null user is not authenticated
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var validatedToken = sessionLogic.ValidateToken(token);
            var userRole = sessionLogic.GetUserRole(validatedToken);

            if (validatedToken == null)
            {
                context.Result = new JsonResult(new { Token = "Invalid Token" })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (!_role.Contains(userRole))
            {
                context.Result = new JsonResult(new { Status = "Unauthorized to perform this action" })
                { StatusCode = StatusCodes.Status403Forbidden };
            }
        }

        protected IAuthService GetAuthService(AuthorizationFilterContext context)
        {
            var sessionHandlerType = typeof(IAuthService);
            var sessionHandlerLogicObject = context.HttpContext.RequestServices.GetService(sessionHandlerType);
            var sessionHandler = sessionHandlerLogicObject as IAuthService;

            return sessionHandler;
        }
    }
}
