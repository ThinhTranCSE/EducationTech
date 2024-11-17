using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Core.Contexts.Interfaces;

public interface IMainDatabaseContext : IDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Role> Roles { get; }
    public DbSet<UserRole> UserRoles { get; }
    public DbSet<UserKey> UserKeys { get; }
    public DbSet<Permission> Permissions { get; }
    public DbSet<RolePermission> RolePermissions { get; }
    public DbSet<Course> Courses { get; }
    public DbSet<Comment> Comments { get; }
    public DbSet<Comunity> Comunities { get; }
    public DbSet<Discussion> Discussions { get; }
    public DbSet<Quiz> Quizzes { get; }
    public DbSet<QuizResult> QuizResults { get; }
    public DbSet<Question> Questions { get; }
    public DbSet<Answer> Answers { get; }
    public DbSet<AnswerLearner> AnswerLearners { get; }
    public DbSet<Video> Videos { get; }
    public DbSet<UploadedFile> UploadedFiles { get; }
    public DbSet<Image> Images { get; }


    //recommendation
    public DbSet<Learner> Learners { get; }
    public DbSet<LearnerLog> LearnerLogs { get; }
    public DbSet<LearningObject> LearningObjects { get; }
    public DbSet<RecommendTopic> RecommendTopics { get; }
    //public DbSet<TopicConjunction> TopicConjunctions { get; }


    public DbSet<CourseGroup> CourseGroups { get; }
    public DbSet<Branch> Branches { get; }
    public DbSet<Speciality> Specialities { get; }
    public DbSet<PrerequisiteCourse> PrerequisiteCourses { get; }
    public DbSet<CourseSpeciality> CourseSpecialities { get; }

    //save learning path
    public DbSet<CourseLearningPathOrder> CourseLearningPathOrders { get; }
    public DbSet<TopicLearningPathOrder> TopicLearningPathOrders { get; }
    public DbSet<LearningObjectLearningPathOrder> LearningObjectLearningPathOrders { get; }
}
