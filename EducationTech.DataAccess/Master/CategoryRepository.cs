using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Master
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(EducationTechContext context) : base(context)
        {
        }

        public override DbSet<Category> Model => _context.Categories;

        public void SaveChanges() => _context.SaveChanges();

    }
}
