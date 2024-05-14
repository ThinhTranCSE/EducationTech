using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Categories;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(EducationTechContext context, IAuthService authService, ICategoryService categoryService) : base(context, authService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> GetCategories()
        {
            return await _categoryService.GetCategories();
        }

        [HttpGet("{treeId}")]
        public async Task<NestedSetRecursiveNodeDto<CategoryDto>> GetCategoryByTreeId(int treeId)
        {
            return await _categoryService.GetCategoryByTreeId(treeId);
        }
    }
}
