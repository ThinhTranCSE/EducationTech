using EducationTech.Auth.Policies.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Auth.Policies
{
    public class UploadCoursePolicy : IPolicy
    {
        public string PolicyName => "UploadCourse";

        public AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder)
        {
            builder.RequireClaim("permissions", "UploadCourse");

            return builder;
        }
    }
}
