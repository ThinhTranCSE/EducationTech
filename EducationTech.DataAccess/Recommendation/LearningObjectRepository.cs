using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace EducationTech.DataAccess.Recommendation;

public class LearningObjectRepository : ILearningObjectRepository
{
    public DbSet<LearningObject> Model => throw new NotImplementedException();

    public Task<LearningObject?> Delete(LearningObject entity, bool executed = false)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<LearningObject>> Delete(IEnumerable<LearningObject> entities, bool executed = false)
    {
        throw new NotImplementedException();
    }

    public EntityEntry<LearningObject> Entry(LearningObject entity)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<LearningObject>> Get(Expression<Func<LearningObject, bool>>? filter = null, bool tracked = true, bool executed = false)
    {
        throw new NotImplementedException();
    }

    public Task<LearningObject?> GetSingle(Expression<Func<LearningObject, bool>> filter, bool tracked = true)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<LearningObject>> Insert(IEnumerable<LearningObject> entities, bool executed = false)
    {
        throw new NotImplementedException();
    }

    public Task<LearningObject?> Insert(LearningObject entity, bool executed = false)
    {
        throw new NotImplementedException();
    }

    public Task<LearningObject?> Update(LearningObject entity, bool executed = false)
    {
        throw new NotImplementedException();
    }
}
