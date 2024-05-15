using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses
{
    public class Course_CreateRequestDto : AbstractDto<Course, Course_CreateRequestDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<int> CategoryIds { get; set; } = new List<int>();
    }
}
