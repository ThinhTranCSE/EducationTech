using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_UpdateRequestDto : AbstractDto<Course, Course_UpdateRequestDto>
    {
        public string? Description { get; set; }
        public string? Title { get; set; }
        //public double? Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsPublished { get; set; }

        public IEnumerable<int> CategoryIds { get; set; } = new List<int>();
    }
}
