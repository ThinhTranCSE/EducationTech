using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class CourseDto : Abstracts.AbstractDto
    {
        public int Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public bool IsArchived { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }
        public override void Configure()
        {
            CreateMap<Course, CourseDto>();
        }
    }

}
