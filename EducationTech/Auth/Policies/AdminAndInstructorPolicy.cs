using EducationTech.Auth.Policies.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Auth.Policies
{
    public class AdminAndInstructorPolicy : IPolicy
    {
        public string PolicyName => "AdminAndInstructor";

        public AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder)
        {
            builder.RequireClaim("roles", "Admin", "Instructor");

            return builder;
        }
    }

}
