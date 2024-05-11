using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Entities.Abstract
{
    public interface INestedSet
    {
        int TreeId { get; set; }
        int Left { get; set; }
        int Right { get; set; }
    }
}
