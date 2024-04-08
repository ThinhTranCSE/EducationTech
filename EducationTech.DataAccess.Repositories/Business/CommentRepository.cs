using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public override DbSet<Comment> Model => _context.Comments;
        public CommentRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
