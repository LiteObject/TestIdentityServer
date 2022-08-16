using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Api
{
    public class DisabledAuthRequirementHandler : AuthorizationHandler<DisabledAuthRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DisabledAuthRequirementHandler(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DisabledAuthRequirement requirement)
        {
            var disableAuth = !_httpContextAccessor.HttpContext.Request.IsHttps;

            if (disableAuth || context.User.Identities.Any(x => x.IsAuthenticated))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
