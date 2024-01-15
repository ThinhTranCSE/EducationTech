using Microsoft.EntityFrameworkCore;

namespace EducationTech.Models.Abstract
{
    public interface IModel
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
