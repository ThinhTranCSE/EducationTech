using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Entities.Abstract
{
    public interface IModel
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
