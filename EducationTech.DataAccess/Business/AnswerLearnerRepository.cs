using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class AnswerLearnerRepository : Repository<AnswerLearner>, IAnswerLearnerRepository
    {
        public AnswerLearnerRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
