namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_GetByIdRequestDto
    {
        public bool BelongToCurrentUser { get; set; } = false;
        public bool IsGetDetail { get; set; } = false;
        public bool IsIncludeRate { get; set; } = false;
    }
}
