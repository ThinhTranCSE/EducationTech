using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Entities.Abstract
{
    public interface IEntity
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
