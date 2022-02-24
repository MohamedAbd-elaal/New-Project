using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_FILE_VIEW_.Security
{
    //custom Handler
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler:
        AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        //anthor way to controll on httpcontext too get the path

        //private readonly IHttpContextAccessor httpContextAccessor;
        //public CanEditOnlyOtherAdminRolesAndClaimsHandler(
        //        IHttpContextAccessor httpContextAccessor)
        //{
        //    this.httpContextAccessor = httpContextAccessor;
        //}
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ManageAdminRolesAndClaimsRequirement requirement)
        {
            // Resource get us the action that we autherize it
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null)
            {
                return Task.CompletedTask;
            }
            string loggedInAdminId =
                   context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
            // string request = httpContextAccessor.HttpContext.Request.Path.Value.ToString();

            //  string adminIdBeingEdited = request.Split('/')[3];
            string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"].ToString();
            if (context.User.IsInRole("user")&&           
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
                adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }//if not success it will fail and get error page after go to another handler
             //but it will succes it even it success because(context.Fail();)
             //but with(option.InvokeHandlersAfterFailure = false; and it is by default is true) it will not go to anthor handler if it has fail then get error page 
            //else
            //{
            //    context.Fail();
            //}
            return Task.CompletedTask;

        }
    }
}
