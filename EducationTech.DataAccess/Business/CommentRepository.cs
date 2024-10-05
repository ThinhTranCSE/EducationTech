using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
