using EducationTech.Auth.Policies.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Auth.Policies
{
    public class InstructorOnlyPolicy : IPolicy
    {
        public string PolicyName => "InstructorOnly";

        public AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder)
        {
            builder.RequireClaim("roles", "Instructor");
            return builder;
        }
    }
}
