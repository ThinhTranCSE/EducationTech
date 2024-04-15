using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public override DbSet<Quiz> Model => _context.Quizzes;
        public QuizRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
