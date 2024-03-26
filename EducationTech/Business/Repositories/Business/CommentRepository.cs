using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public override DbSet<Comment> Model => _context.Comments;
        public CommentRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
