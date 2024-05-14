using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Shared.NestedSet
{
    public interface INestedSetNode
    {
        int TreeId { get; set; }
        int Left { get; set; }
        int Right { get; set; }
    }
}
