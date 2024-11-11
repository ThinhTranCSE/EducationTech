using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_UpdateRequestDto : AbstractDto<Course, Course_UpdateRequestDto>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsPublished { get; set; }
        public string? ImageUrl { get; set; }
        public string? CourseCode { get; set; }
        public int? Credits { get; set; }
        public int? RecommendedSemester { get; set; }
        public int? CourseGroupId { get; set; }
        public ICollection<int>? SpecialityIds { get; set; }

        public override void Configure(AutoMapper.IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Course_UpdateRequestDto, Course>()
                .ForAllMembers(x => x.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
