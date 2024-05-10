using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Business.LearnerCourses
{
    public class LearnerCourseDto : Abstracts.AbstractDto<LearnerCourse, LearnerCourseDto>
    {
        public int Id { get; set; }

        public Guid LearnerId { get; set; }
        public virtual UserDto Learner { get; set; }

        public int CourseId { get; set; }
        public virtual CourseDto Course { get; set; }

        public double Rate { get; set; }
    }
}
