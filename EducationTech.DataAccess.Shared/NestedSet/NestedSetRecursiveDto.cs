using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Shared.NestedSet
{
    public class NestedSetRecursiveDto
    {
        public INestedSetNode Node { get; set; }
        public IEnumerable<NestedSetRecursiveDto> Children { get; set; }
    }
}
