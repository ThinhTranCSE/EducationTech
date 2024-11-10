using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Neo4jClient;
using Neo4jClient.DataAnnotations;

namespace EducationTech.DataAccess.Abstract;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMainDatabaseContext _context;
    private readonly IGraphClient _graphClient;
    public IGraphClient GraphClient => _graphClient.WithAnnotations();
    #region Business Repositories
    public IAnswerRepository Answers { get; private set; }
    public IAnswerUserRepository AnswerUsers { get; private set; }
    public ICommentRepository Comments { get; private set; }
    public IComunityRepository Comunities { get; private set; }
    public IImageRepository Images { get; private set; }
    public IQuestionRepository Questions { get; private set; }
    public IQuizRepository Quizzes { get; private set; }
    public ITopicRepository Topics { get; private set; }
    public IUploadedFileRepository UploadedFiles { get; private set; }
    public IUserKeyRepository UserKeys { get; private set; }
    public IVideoRepository Videos { get; private set; }

    #endregion


    #region Master Repositories
    public ICourseRepository Courses { get; private set; }
    public ICourseSectionRepository CourseSections { get; private set; }
    public ILessonRepository Lessons { get; private set; }
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
    public ITopicConjunctionRepository TopicConjunctions { get; private set; }

    public IBranchRepository Branches { get; private set; }
    public ISpecialityRepository Specialities { get; private set; }
    public ICourseGroupRepository CourseGroups { get; private set; }
    #endregion


    public UnitOfWork(
        IGraphClient graphClient,
        IMainDatabaseContext context,
        IAnswerRepository answerRepository,
        IAnswerUserRepository answerUserRepository,
        ICommentRepository commentRepository,
        IComunityRepository comunityRepository,
        IImageRepository imageRepository,
        IQuestionRepository questionRepository,
        IQuizRepository quizRepository,
        ITopicRepository topicRepository,
        IUploadedFileRepository uploadedFileRepository,
        IUserKeyRepository userKeyRepository,
        IVideoRepository videoRepository,
        ICourseRepository courseRepository,
        ICourseSectionRepository courseSectionRepository,
        ILessonRepository lessonRepository,
        IPermissionRepository permissionRepository,
        IRolePermissionRepository rolePermissionRepository,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        ILearnerLogRepository learnerLogRepository,
        ILearnerRepository learnerRepository,
        ILearningObjectRepository learningObjectRepository,
        IRecommendTopicRepository recommendTopicRepository,
        ITopicConjunctionRepository topicConjunctionRepository,
        ISpecialityRepository specialityRepository,
        ICourseGroupRepository courseGroups,
        IBranchRepository branchRepository

        )
    {
        _graphClient = graphClient;
        _context = context;
        Answers = answerRepository;
        AnswerUsers = answerUserRepository;
        Comments = commentRepository;
        Comunities = comunityRepository;
        Images = imageRepository;
        Questions = questionRepository;
        Quizzes = quizRepository;
        Topics = topicRepository;
        UploadedFiles = uploadedFileRepository;
        UserKeys = userKeyRepository;
        Videos = videoRepository;
        Courses = courseRepository;
        CourseSections = courseSectionRepository;
        Lessons = lessonRepository;
        Permissions = permissionRepository;
        RolePermissions = rolePermissionRepository;
        Roles = roleRepository;
        Users = userRepository;
        UserRoles = userRoleRepository;
        LearnerLogs = learnerLogRepository;
        Learners = learnerRepository;
        LearningObjects = learningObjectRepository;
        RecommendTopics = recommendTopicRepository;
        TopicConjunctions = topicConjunctionRepository;
        Specialities = specialityRepository;
        CourseGroups = courseGroups;
        Branches = branchRepository;
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
