using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Master;
using EducationTech.DataAccess.Master.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class CategorySeeder : Seeder
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategorySeeder(EducationTechContext context, ICategoryRepository categoryRepository) : base(context)
        {
            _categoryRepository = categoryRepository;
        }

        public override void Seed()
        {
            throw new NotImplementedException();
        }
    }
}
