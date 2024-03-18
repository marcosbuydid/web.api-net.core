using Demo.Web.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Web.API.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionLogic = this.GetAuthService(context);

            //If token is null user is not authenticated
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (sessionLogic.ValidateToken(token) == null)
            {
                context.Result = new ObjectResult(new { Message = "Authorization required" })
                {
                    StatusCode = 401
                };
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
