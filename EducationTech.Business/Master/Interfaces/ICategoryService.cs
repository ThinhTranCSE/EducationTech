using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Categories;
using EducationTech.DataAccess.Shared.NestedSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ICategoryService : IService
    {
        Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> GetCategories();
        Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> DeleteCategories(int id);
    }
}
