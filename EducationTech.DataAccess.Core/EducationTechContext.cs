using EducationTech.DataAccess.Core.Extensions;
using EducationTech.DataAccess.Core.Interceptors;
using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EducationTech.DataAccess.Core
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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerUser> AnswerUsers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }

        //recommendation
        public DbSet<Learner> Learners { get; set; }
        public DbSet<LearnerLog> LearnerLogs { get; set; }
        public DbSet<LearningObject> LearningObjects { get; set; }
        public DbSet<LearningStyle> LearningStyles { get; set; }
        public DbSet<RecommendTopic> RecommendTopics { get; set; }
        public DbSet<TopicConjunction> TopicConjunctions { get; set; }

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
            var modelImplementationTypes = typeof(Entity).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(Entity)) && !t.IsInterface && !t.IsAbstract)
                .ToList();
            modelImplementationTypes.ForEach(modelImplementation =>
            {
                modelImplementation
                    .GetMethod(nameof(IEntity.OnModelCreating))
                    ?.Invoke(Activator.CreateInstance(modelImplementation), new object[] { builder });
            });
        }
    }
}
