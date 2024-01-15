using EducationTech.Databases.Factories;
using EducationTech.Models.Abstract;
using EducationTech.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases
{
    public class MainDatabaseContext : DbContext
    {
        public MainDatabaseContext(DbContextOptions<MainDatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.HasDefaultSchema("master");
            //builder.ApplyConfigurationsFromAssembly(typeof(MainDatabaseContext).Assembly);
            InvokeModelCreating(builder);
            base.OnModelCreating(builder);
        }

        private void InvokeModelCreating(ModelBuilder builder)
        {
            var modelImplementationTypes = typeof(Model).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(Model)) && !t.IsInterface && !t.IsAbstract)
                .ToList();
            modelImplementationTypes.ForEach(modelImplementation =>
            {
                modelImplementation
                    .GetMethod(nameof(IModel.OnModelCreating))
                    ?.Invoke(Activator.CreateInstance(modelImplementation), new object[] { builder });

            });
        }
    }
}
