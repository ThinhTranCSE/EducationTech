using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Shared.NestedSet;

namespace EducationTech.DataAccess.Business.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>, INestedSet<Comment>
    {
    }
}
