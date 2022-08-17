using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Api
{
    /// <summary>
    /// Underneath the covers, role-based authorization and claims-based authorization use: 
    ///     1. a requirement, 
    ///     2. a requirement handler, and 
    ///     3. a preconfigured policy
    /// These building blocks support the expression of authorization evaluations in code. 
    /// 
    /// An authorization policy consists of one or more requirements and all of the requirements 
    /// must be satisfied for a policy to succeed.
    /// 
    /// More info:    
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0
    /// </summary>
    public class DisabledAuthRequirementHandler : AuthorizationHandler<DisabledAuthRequirement>
    {
        // private readonly IHttpContextAccessor _httpContextAccessor;

        public DisabledAuthRequirementHandler() 
        {
            // _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// A requirement can have multiple handlers. A handler may inherit AuthorizationHandler<TRequirement>, 
        /// where TRequirement is the requirement to be handled. Alternatively, a handler may implement 
        /// IAuthorizationHandler directly to handle more than one type of requirement.
        /// 
        /// Authorization handlers are called even if authentication fails.
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DisabledAuthRequirement requirement)
        {
            /*
             * A handler doesn't need to handle failures generally, 
             * as other handlers for the same requirement may succeed.
             */
            
            var userAgent = string.Empty;

            // In this case, the Resource property is an instance of HttpContext. The context can be used to access the current endpoint
            if (context.Resource is HttpContext httpContext)
            {
                Console.WriteLine("-------------------HEADERS---------------------");
                foreach (var h in httpContext.Request.Headers) 
                {
                    Console.WriteLine(">>> " + h.Key + ": " + h.Value);
                }
                Console.WriteLine("-----------------------------------------------");

                // var actionDescriptor = httpContext.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
                userAgent = httpContext.Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.UserAgent].ToString();
            }

            if (userAgent == requirement.UserAgent || context.User.Identities.Any(x => x.IsAuthenticated))
            {
                // context.Fail();
                context.Succeed(requirement);                
            }

            return Task.CompletedTask;
        }
    }
}
