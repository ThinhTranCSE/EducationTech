using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Auth.Policies.Abstract
{
    public interface IPolicy
    {
        string PolicyName { get; }
        AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder);
    }
}
