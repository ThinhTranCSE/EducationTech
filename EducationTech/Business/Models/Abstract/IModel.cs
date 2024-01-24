using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Models.Abstract
{
    public interface IModel
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
