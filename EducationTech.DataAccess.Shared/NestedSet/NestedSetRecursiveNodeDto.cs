using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Shared.NestedSet
{
    public class NestedSetRecursiveNodeDto<TNestedSetNode>
        where TNestedSetNode : class, INestedSetNode
    {
        public TNestedSetNode Node { get; set; }
        public ICollection<NestedSetRecursiveNodeDto<TNestedSetNode>> Children { get; set; } = new List<NestedSetRecursiveNodeDto<TNestedSetNode>>();
    }
}
