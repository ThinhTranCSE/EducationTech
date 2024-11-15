namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_GetRequestDto
    {
        public int? Limit { get; set; }
        public int? Offset
        {
            get; set;
        }
        public ICollection<int> SpecialityIds { get; set; } = new List<int>();
    }
}
