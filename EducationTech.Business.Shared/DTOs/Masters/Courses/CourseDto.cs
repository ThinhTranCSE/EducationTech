using AutoMapper;
using EducationTech.Business.Shared.DTOs.Masters.CourseCategories;
using EducationTech.Business.Shared.DTOs.Masters.CourseSections;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Storage;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class CourseDto : Abstracts.AbstractDto<Course, CourseDto>
    {
        public int Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool IsArchived { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishedAt { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public UserDto Owner { get; set; }
        public double? Rate { get; set; } = null;
        public ICollection<CourseSectionDto> CourseSections { get; set; }
        public ICollection<CourseCategoryDto> CourseCategories { get; set; }
        public override void Configure(IMapperConfigurationExpression cfg)
        {
            string hostName = GlobalReference.Instance.HostName;
            string scheme = GlobalReference.Instance.HostScheme;
            cfg.CreateMap<Course, CourseDto>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom<ImageUrlResolver>())
                .ReverseMap();
        }
    }

    public class ImageUrlResolver : IValueResolver<Course, CourseDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Course source, CourseDto destination, string destMember, ResolutionContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                string scheme = httpContext.Request.Scheme;
                string hostName = httpContext.Request.Host.Value;
                return $"{scheme}://{hostName}/{source.ImageUrl}";
            }
            return source.ImageUrl; // Return original if HttpContext is null
        }
    }
}
