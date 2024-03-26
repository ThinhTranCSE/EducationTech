using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public override DbSet<Question> Model => _context.Questions;
        public QuestionRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
