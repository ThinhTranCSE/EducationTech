using EducationTech.Enums;
using EducationTech.Policies.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EducationTech.Policies
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
