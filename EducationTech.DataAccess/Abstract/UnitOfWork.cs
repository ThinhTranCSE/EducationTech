using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationTech.DataAccess.Abstract;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMainDatabaseContext _context;
    //private readonly IGraphClient _graphClient;
    //public IGraphClient GraphClient => _graphClient.WithAnnotations();
    #region Business Repositories
    public IAnswerRepository Answers { get; private set; }
    public IAnswerLearnerRepository AnswerLearners { get; private set; }
    public ICommentRepository Comments { get; private set; }
    public IComunityRepository Comunities { get; private set; }

    public IDiscussionRepository Discussions { get; private set; }
    public IImageRepository Images { get; private set; }
    public IQuestionRepository Questions { get; private set; }
    public IQuizRepository Quizzes { get; private set; }

    public IQuizResultRepository QuizResults { get; private set; }
    public IUploadedFileRepository UploadedFiles { get; private set; }
    public IUserKeyRepository UserKeys { get; private set; }
    public IVideoRepository Videos { get; private set; }

    #endregion


    #region Master Repositories
    public ICourseRepository Courses { get; private set; }
    public IPermissionRepository Permissions { get; private set; }
    public IRolePermissionRepository RolePermissions { get; private set; }
    public IRoleRepository Roles { get; private set; }
    public IUserRepository Users { get; private set; }
    public IUserRoleRepository UserRoles { get; private set; }

    #endregion

    #region Recommendation Repositories

    public ILearnerLogRepository LearnerLogs { get; private set; }
    public ILearnerRepository Learners { get; private set; }
    public ILearningObjectRepository LearningObjects { get; private set; }
    public IRecommendTopicRepository RecommendTopics { get; private set; }
    //public ITopicConjunctionRepository TopicConjunctions { get; private set; }

    public IBranchRepository Branches { get; private set; }
    public ISpecialityRepository Specialities { get; private set; }
    public ICourseGroupRepository CourseGroups { get; private set; }
    public IPrerequisiteCourseRepository PrerequisiteCourses { get; private set; }

    public ICourseSpecialityRepository CourseSpecialities { get; private set; }

    public ICourseLearningPathOrderRepository CourseLearningPathOrders { get; private set; }
    public ITopicLearningPathOrderRepository TopicLearningPathOrders { get; private set; }
    public ILearningObjectLearningPathOrderRepository LearningObjectLearningPathOrders { get; private set; }
    #endregion


    public UnitOfWork(
        //IGraphClient graphClient,
        IMainDatabaseContext context,
        IAnswerRepository answerRepository,
        IAnswerLearnerRepository answerUserRepository,
        ICommentRepository commentRepository,
        IComunityRepository comunityRepository,
        IDiscussionRepository discussionRepository,
        IImageRepository imageRepository,
        IQuestionRepository questionRepository,
        IQuizRepository quizRepository,
        IQuizResultRepository quizResultRepository,
        IUploadedFileRepository uploadedFileRepository,
        IUserKeyRepository userKeyRepository,
        IVideoRepository videoRepository,
        ICourseRepository courseRepository,
        IPermissionRepository permissionRepository,
        IRolePermissionRepository rolePermissionRepository,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        ILearnerLogRepository learnerLogRepository,
        ILearnerRepository learnerRepository,
        ILearningObjectRepository learningObjectRepository,
        IRecommendTopicRepository recommendTopicRepository,
        //ITopicConjunctionRepository topicConjunctionRepository,
        ISpecialityRepository specialityRepository,
        ICourseGroupRepository courseGroups,
        IPrerequisiteCourseRepository prerequisiteCourseRepository,
        IBranchRepository branchRepository,
        ICourseSpecialityRepository courseSpecialityRepository,
        ICourseLearningPathOrderRepository courseLearningPathOrderRepository,
        ITopicLearningPathOrderRepository topicLearningPathOrderRepository,
        ILearningObjectLearningPathOrderRepository learningObjectLearningPathOrderRepository

        )
    {
        //_graphClient = graphClient;
        _context = context;
        Answers = answerRepository;
        AnswerLearners = answerUserRepository;
        Comments = commentRepository;
        Comunities = comunityRepository;
        Discussions = discussionRepository;
        Images = imageRepository;
        Questions = questionRepository;
        Quizzes = quizRepository;
        QuizResults = quizResultRepository;
        UploadedFiles = uploadedFileRepository;
        UserKeys = userKeyRepository;
        Videos = videoRepository;
        Courses = courseRepository;
        Permissions = permissionRepository;
        RolePermissions = rolePermissionRepository;
        Roles = roleRepository;
        Users = userRepository;
        UserRoles = userRoleRepository;
        LearnerLogs = learnerLogRepository;
        Learners = learnerRepository;
        LearningObjects = learningObjectRepository;
        RecommendTopics = recommendTopicRepository;
        //TopicConjunctions = topicConjunctionRepository;
        Specialities = specialityRepository;
        CourseGroups = courseGroups;
        PrerequisiteCourses = prerequisiteCourseRepository;
        Branches = branchRepository;
        CourseSpecialities = courseSpecialityRepository;
        CourseLearningPathOrders = courseLearningPathOrderRepository;
        TopicLearningPathOrders = topicLearningPathOrderRepository;
        LearningObjectLearningPathOrders = learningObjectLearningPathOrderRepository;
    }


    public IDbContextTransaction BeginTransaction()
    {
        return _context.Instance.Database.BeginTransaction();
    }

    public void Dispose() => _context.Dispose();

    public int SaveChanges()
    {
        return _context.Instance.SaveChanges();
    }

    EntityEntry<TEntity> IUnitOfWork.Entry<TEntity>(TEntity entity)
    {
        return _context.Instance.Entry(entity);
    }
}
