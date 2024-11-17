using AutoMapper;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.Business.Shared.DTOs.Recommendation.CourseGroups;
using EducationTech.Business.Shared.DTOs.Recommendation.CourseSpecialities;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Storage;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class CourseDto : Abstracts.AbstractDto<Course, CourseDto>
    {
        public int Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? CourseGroupId { get; set; }
        public string CourseCode { get; set; }
        public int Credits { get; set; }
        public int RecommendedSemester { get; set; }
        public CourseGroupDto? CourseGroup { get; set; }
        public UserDto Owner { get; set; }
        public ICollection<RecommendTopicDto> Topics { get; set; } = new List<RecommendTopicDto>();
        public ICollection<CourseSpecialityDto> Specialities { get; set; } = new List<CourseSpecialityDto>();
        public ICollection<PrerequisiteCourseDto> Prerequisites { get; set; } = new List<PrerequisiteCourseDto>();
        public override void Configure(IMapperConfigurationExpression cfg)
        {
            string hostName = GlobalReference.Instance.HostName;
            string scheme = GlobalReference.Instance.HostScheme;
            cfg.CreateMap<Course, CourseDto>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => $"{scheme}://{hostName}/{x.ImageUrl}"))
                .ReverseMap();
        }
    }
}
