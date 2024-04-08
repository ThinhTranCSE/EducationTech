using System.Diagnostics.Eventing.Reader;

namespace EducationTech.Business.Shared.DTOs.Masters.User
{
    public class User_GetDto
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid>? Ids { get; set; }
        public string? Username { get; set; }

        public bool IsIncludeRoles { get; set; }

        public bool IsIncludeKey { get; set; }
    }
}
