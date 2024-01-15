using Microsoft.EntityFrameworkCore;

namespace EducationTech.Models.Abstract
{
    public abstract class Model : IModel
    {
        public abstract void OnModelCreating(ModelBuilder modelBuilder);
    }
}
