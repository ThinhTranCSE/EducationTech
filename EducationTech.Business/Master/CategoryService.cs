using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Categories;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> GetCategories()
        {

            var query = await _categoryRepository.Get();
            List<Category> categories = await query.ToListAsync()!;
            int left = categories.Min(x => x.Left) - 1;


            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            var trees = categoryDtos.ToTrees(left);

            return trees;
        }

        public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> DeleteCategories(int id)
        {
            var query = await _categoryRepository.Get();
            query = query.Where(x => x.Id == id);

            var category = await query.FirstOrDefaultAsync()!;

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            var categoryTree = _categoryRepository.GetTree(category);
            int left = category.Left - 1;

            _categoryRepository.RemoveNode(category);

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categoryTree);
           

            var tree = categoryDtos.ToTrees(left);

            return tree;
        }
    }
}
