using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Shared.NestedSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Categories
{
    public class CategoryDto : AbstractDto<Category, CategoryDto>, INestedSetNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TreeId { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int? ParentId { get; set; }
    }
}
