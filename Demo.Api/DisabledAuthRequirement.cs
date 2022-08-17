using Microsoft.AspNetCore.Authorization;

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
    public class DisabledAuthRequirement : IAuthorizationRequirement
    {
        // A requirement doesn't need to have data or properties.

        public string UserAgent { get; set; } = string.Empty;
    }
}
