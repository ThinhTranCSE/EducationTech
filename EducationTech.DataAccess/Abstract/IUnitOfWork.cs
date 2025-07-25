﻿using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationTech.DataAccess.Abstract;

public interface IUnitOfWork : IDisposable
{
    //IGraphClient GraphClient { get; }
    int SaveChanges();
    IDbContextTransaction BeginTransaction();
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity;

    #region Business Repositories
    IAnswerRepository Answers { get; }
    IAnswerLearnerRepository AnswerLearners { get; }
    ICommentRepository Comments { get; }
    IComunityRepository Comunities { get; }
    IDiscussionRepository Discussions { get; }
    IImageRepository Images { get; }
    IQuestionRepository Questions { get; }
    IQuizRepository Quizzes { get; }

    IQuizResultRepository QuizResults { get; }
    IUploadedFileRepository UploadedFiles { get; }
    IUserKeyRepository UserKeys { get; }
    IVideoRepository Videos { get; }

    #endregion

    #region Master Repositories
    ICourseRepository Courses { get; }
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
    //ITopicConjunctionRepository TopicConjunctions { get; }

    IBranchRepository Branches { get; }
    ISpecialityRepository Specialities { get; }
    ICourseGroupRepository CourseGroups { get; }
    ICourseSpecialityRepository CourseSpecialities { get; }
    IPrerequisiteCourseRepository PrerequisiteCourses { get; }

    ICourseLearningPathOrderRepository CourseLearningPathOrders { get; }
    ITopicLearningPathOrderRepository TopicLearningPathOrders { get; }
    ILearningObjectLearningPathOrderRepository LearningObjectLearningPathOrders { get; }
    #endregion
}
