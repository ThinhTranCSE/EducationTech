using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Categories;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(EducationTechContext context, IAuthService authService, ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> GetCategories()
    {
        return await _categoryService.GetCategories();
    }

    [HttpDelete("{id}")]
    public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> DeleteCategories(int id)
    {
        return await _categoryService.DeleteCategories(id);
    }

}
