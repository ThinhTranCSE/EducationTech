using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMainDatabaseContext context) : base(context)
        {
        }

        public DbSet<Category> EntityNode => _context.Categories;

        public void SaveChanges() => _context.Instance.SaveChanges();

    }
}
