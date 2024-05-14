using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Shared.NestedSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Master.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>, INestedSet<Category>
    {
    }
}
