using Microsoft.AspNetCore.Authorization;

namespace Demo.Api
{
    /// <summary>
    /// This is a requirement class. This class will be used to discable
    /// auth in certain caeses.
    /// 
    /// - An authorization policy is made of one or more requirements
    /// - All of the requirements must be satisfied for a policy to succeed
    /// 
    /// More info:
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-2.2#requirements-1
    /// </summary>
    public class DisabledAuthRequirement : IAuthorizationRequirement
    {
    }
}
