using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class BranchRepository : Repository<Branch>, IBranchRepository
{
    public BranchRepository(IMainDatabaseContext context) : base(context)
    {
    }
}
