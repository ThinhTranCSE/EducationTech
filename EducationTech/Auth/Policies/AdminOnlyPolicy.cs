using EducationTech.Auth.Policies.Abstract;
using EducationTech.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EducationTech.Auth.Policies
{
    public class AdminOnlyPolicy : IPolicy
    {
        public string PolicyName => "AdminOnly";

        public AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder)
        {
            builder.RequireClaim("roles", nameof(RoleType.Admin));
            return builder;
        }
    }
}
