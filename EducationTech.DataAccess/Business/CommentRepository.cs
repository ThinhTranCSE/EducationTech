using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(IMainDatabaseContext context) : base(context)
        {
        }

        public DbSet<Comment> EntityNode => _context.Comments;

        public void SaveChanges() => _context.Instance.SaveChanges();

    }
}
