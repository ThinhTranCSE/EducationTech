namespace EducationTech.DTOs.Masters.User
{
    public class UserGetDto
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid>? Ids { get; set; }
        public string? Username { get; set; }

    }
}
