using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Neo4jClient;

namespace EducationTech.DataAccess.Abstract;

public interface IUnitOfWork : IDisposable
{
    IGraphClient GraphClient { get; }
    int SaveChanges();
    IDbContextTransaction BeginTransaction();
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity;

    #region Business Repositories
    IAnswerRepository Answers { get; }
    IAnswerUserRepository AnswerUsers { get; }
    ICommentRepository Comments { get; }
    IComunityRepository Comunities { get; }
    IImageRepository Images { get; }
    IQuestionRepository Questions { get; }
    IQuizRepository Quizzes { get; }
    ITopicRepository Topics { get; }
    IUploadedFileRepository UploadedFiles { get; }
    IUserKeyRepository UserKeys { get; }
    IVideoRepository Videos { get; }

    #endregion

    #region Master Repositories
    ICourseRepository Courses { get; }
    ICourseSectionRepository CourseSections { get; }
    ILessonRepository Lessons { get; }
    IPermissionRepository Permissions { get; }
    IRolePermissionRepository RolePermissions { get; }
    IRoleRepository Roles { get; }
    IUserRepository Users { get; }
    IUserRoleRepository UserRoles { get; }
    #endregion

    #region Recommendation Repositories
    ILearnerLogRepository LearnerLogs { get; }
    ILearnerRepository Learners { get; }
    ILearningObjectRepository LearningObjects { get; }
    IRecommendTopicRepository RecommendTopics { get; }
    ITopicConjunctionRepository TopicConjunctions { get; }

    IBranchRepository Branches { get; }
    ISpecialityRepository Specialities { get; }
    ICourseGroupRepository CourseGroups { get; }
    #endregion
}
