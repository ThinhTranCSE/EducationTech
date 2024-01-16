using EducationTech.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EducationTech.Databases.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if(eventData.Context == null ) return result;

            var entries = eventData.Context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                Model model = entry.Entity as Model;
                if(entry.State != EntityState.Deleted || !model.SoftDelete) continue;

                entry.State = EntityState.Modified;
                model.DeletedAt = DateTimeOffset.UtcNow;
            }

            return result;
        }
    }
}
