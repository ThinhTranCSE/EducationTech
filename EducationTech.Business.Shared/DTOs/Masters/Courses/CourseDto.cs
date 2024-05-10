using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void Configure(IMapperConfigurationExpression cfg)
        {
            string hostName = AbstractDto<Course, CourseDto>.GlobalUsings.HostName;
            string scheme = AbstractDto<Course, CourseDto>.GlobalUsings.HostScheme;
            cfg.CreateMap<Course, CourseDto>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => $"{scheme}://{hostName}/{x.ImageUrl}"))
                .ReverseMap();

        }
    }

}
