using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public QuizRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
