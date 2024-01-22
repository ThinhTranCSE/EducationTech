using Microsoft.AspNetCore.Authorization;

namespace EducationTech.Policies.Abstract
{
    public interface IPolicy
    {
        string PolicyName { get; }
        AuthorizationPolicyBuilder ApplyRequirements(AuthorizationPolicyBuilder builder);
    }
}
