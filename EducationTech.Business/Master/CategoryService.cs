using AutoMapper;
using EducationTech.DataAccess.Abstract;

namespace EducationTech.Business.Master
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> GetCategories()
        //{

        //    var query = _unitOfWork.Categories.GetAll();
        //    List<Category> categories = await query.ToListAsync()!;
        //    int left = categories.Min(x => x.Left) - 1;


        //    var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

        //    var trees = categoryDtos.ToTrees(left);

        //    return trees;
        //}

        //public async Task<IEnumerable<NestedSetRecursiveNodeDto<CategoryDto>>> DeleteCategories(int id)
        //{
        //    var query = _unitOfWork.Categories.GetAll();
        //    query = query.Where(x => x.Id == id);

        //    var category = await query.FirstOrDefaultAsync()!;

        //    if (category == null)
        //    {
        //        throw new HttpException(HttpStatusCode.NotFound, "Category not found");
        //    }

        //    var categoryTree = _unitOfWork.Categories.GetTree(category);
        //    int left = category.Left - 1;

        //    _unitOfWork.Categories.RemoveNode(category);

        //    var categoryDtos = _mapper.Map<List<CategoryDto>>(categoryTree);


        //    var tree = categoryDtos.ToTrees(left);

        //    return tree;
        //}
    }
}
