using EducationTech.Auth.Policies.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Auth.Policies
{
    public class UpdateCoursePolicy : IPolicy
    {
        public string PolicyName => "UpdateCourse";

        public AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder)
        {
            builder.RequireClaim("permissions", "UpdateCourse");

            return builder;
        }
    }
}
