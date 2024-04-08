using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public override DbSet<Question> Model => _context.Questions;
        public QuestionRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
