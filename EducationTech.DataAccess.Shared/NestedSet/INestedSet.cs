using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Shared.NestedSet
{
    public interface INestedSet<TNestedSetNode>
        where TNestedSetNode : class, INestedSetNode
    {
        DbSet<TNestedSetNode> EntityNode { get; }
        void SaveChanges();
    }
}
