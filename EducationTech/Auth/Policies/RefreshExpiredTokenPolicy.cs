using EducationTech.Auth.Policies.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Auth.Policies
{
    public class RefreshExpiredTokenPolicy : IPolicy
    {
        public string PolicyName => "RefreshExpiredToken";

        public AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder)
        {
            builder.RequireClaim("refresh", "true");

            return builder;
        }
    }
}
