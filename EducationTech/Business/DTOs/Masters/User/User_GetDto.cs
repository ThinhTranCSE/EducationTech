namespace EducationTech.Business.DTOs.Masters.User
{
    public class User_GetDto
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid>? Ids { get; set; }
        public string? Username { get; set; }

    }
}
