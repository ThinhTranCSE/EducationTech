using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Business
{
    public class CourseCategoryRepository : Repository<CourseCategory>, ICourseCategoryRepository
    {
        public CourseCategoryRepository(EducationTechContext context) : base(context)
        {
        }

        public override DbSet<CourseCategory> Model => _context.CourseCategories;
    }
}
