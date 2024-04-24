using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EducationTech.DataAccess.Core.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context == null) return result;

            var entries = eventData.Context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                Entity model = entry.Entity as Entity;
                if (entry.State != EntityState.Deleted || !model.SoftDelete) continue;

                entry.State = EntityState.Modified;
                model.DeletedAt = DateTimeOffset.UtcNow;
            }

            return result;
        }
    }
}
