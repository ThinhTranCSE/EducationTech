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

    #region Business Repositories
    public IAnswerRepository Answers { get; private set; }
    public IAnswerUserRepository AnswerUsers { get; private set; }
    public ICommentRepository Comments { get; private set; }
    public IComunityRepository Comunities { get; private set; }
    public ICourseCategoryRepository CourseCategories { get; private set; }
    public IImageRepository Images { get; private set; }
    public IInstructorApprovedRepository InstructorApproveds { get; private set; }
    public ILearnerCourseRepository LearnerCourses { get; private set; }
    public IQuestionRepository Questions { get; private set; }
    public IQuizRepository Quizzes { get; private set; }
    public ITopicRepository Topics { get; private set; }
    public IUploadedFileRepository UploadedFiles { get; private set; }
    public IUserKeyRepository UserKeys { get; private set; }
    public IVideoRepository Videos { get; private set; }

    #endregion


    #region Master Repositories
    public ICategoryRepository Categories { get; private set; }
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
    public ILearningStyleRepository LearningStyles { get; private set; }
    public IRecommendTopicRepository RecommendTopics { get; private set; }
    public ITopicConjunctionRepository TopicConjunctions { get; private set; }
    #endregion


    public UnitOfWork(
        IMainDatabaseContext context,
        IAnswerRepository answerRepository,
        IAnswerUserRepository answerUserRepository,
        ICommentRepository commentRepository,
        IComunityRepository comunityRepository,
        ICourseCategoryRepository courseCategoryRepository,
        IImageRepository imageRepository,
        IInstructorApprovedRepository instructorApprovedRepository,
        ILearnerCourseRepository learnerCourseRepository,
        IQuestionRepository questionRepository,
        IQuizRepository quizRepository,
        ITopicRepository topicRepository,
        IUploadedFileRepository uploadedFileRepository,
        IUserKeyRepository userKeyRepository,
        IVideoRepository videoRepository,
        ICategoryRepository categoryRepository,
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
        ILearningStyleRepository learningStyleRepository,
        IRecommendTopicRepository recommendTopicRepository,
        ITopicConjunctionRepository topicConjunctionRepository

        )
    {
        _context = context;
        Answers = answerRepository;
        AnswerUsers = answerUserRepository;
        Comments = commentRepository;
        Comunities = comunityRepository;
        CourseCategories = courseCategoryRepository;
        Images = imageRepository;
        InstructorApproveds = instructorApprovedRepository;
        LearnerCourses = learnerCourseRepository;
        Questions = questionRepository;
        Quizzes = quizRepository;
        Topics = topicRepository;
        UploadedFiles = uploadedFileRepository;
        UserKeys = userKeyRepository;
        Videos = videoRepository;
        Categories = categoryRepository;
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
        LearningStyles = learningStyleRepository;
        RecommendTopics = recommendTopicRepository;
        TopicConjunctions = topicConjunctionRepository;
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
