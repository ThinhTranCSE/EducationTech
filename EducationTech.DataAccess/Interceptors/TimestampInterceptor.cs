using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EducationTech.DataAccess.Core.Interceptors
{
    public class TimestampInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context == null) return result;

            var entries = eventData.Context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                Model model = entry.Entity as Model;
                if (!model.Timestamp) continue;
                switch (entry.State)
                {
                    case EntityState.Added:
                        model.CreatedAt = DateTimeOffset.UtcNow;
                        break;
                    case EntityState.Modified:
                        model.UpdatedAt = DateTimeOffset.UtcNow;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
