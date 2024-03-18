using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public override DbSet<Quiz> Model => _context.Quizzes;
        public QuizRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
