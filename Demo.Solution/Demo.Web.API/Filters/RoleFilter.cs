using Demo.Web.API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Web.API.Filters
{
    public class RoleFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] appRoles = new string[2];

        public RoleFilter(params string[] roles)
        {
            appRoles = roles;
            appRoles[0] = "Administrator";
           
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionLogic = this.GetAuthService(context);

            //If token is null user is not authenticated
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validatedToken = sessionLogic.ValidateToken(token);
            var userRole = sessionLogic.GetUserRole(validatedToken);
            for(int i = 0; i< appRoles.Length; i++)
            {
                if (!appRoles[i].Contains(userRole))
                {
                    context.Result = new ObjectResult(new { Message = "Unauthorized to perform this action" })
                    {
                        StatusCode = 403
                    };
                }
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
