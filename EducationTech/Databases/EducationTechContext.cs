using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Business;
using EducationTech.Business.Models.Master;
using EducationTech.Databases.Factories;
using EducationTech.Databases.Interceptors;
using EducationTech.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Databases
{
    public class EducationTechContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public EducationTechContext(DbContextOptions<EducationTechContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserKey> UserKeys { get; set; }
        public DbSet<Permission> Permissions { get; set; } 
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<LearnerCourse> LearnerCourses { get; set; }
        public DbSet<InstructorApproved> InstructorApproveds { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Comunity> Comunities { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }    
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DbSet<UploadedFile> UploadedFiles { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .RegisterDbContext(_configuration)
                .AddInterceptors(
                    new SoftDeleteInterceptor(), 
                    new TimestampInterceptor(), 
                    new LogSlowQueriesInterceptor(500),
                    new LogQueriesInterceptor()
                );

            base.OnConfiguring(optionsBuilder);
        }

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
