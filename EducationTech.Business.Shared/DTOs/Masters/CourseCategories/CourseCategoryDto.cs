using EducationTech.Business.Shared.DTOs.Masters.Categories;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.CourseCategories
{
    public class CourseCategoryDto : Abstracts.AbstractDto<CourseCategory, CourseCategoryDto>
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
    }
}
