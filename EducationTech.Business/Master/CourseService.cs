using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Comunities;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EducationTech.Business.Master
{
    public class CourseService : ICourseService, IPagination<Course_GetRequestDto, Course_GetResponseDto>
    {
        IUnitOfWork _unitOfWork;
        ISessionService _sessionService;
        private readonly IMapper _mapper;
        public CourseService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ISessionService sessionService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sessionService = sessionService;
        }

        public async Task<CourseDto> CreateCourse(Course_CreateRequestDto requestDto)
        {
            var course = _mapper.Map<Course>(requestDto);

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                string pattern = @"^(?:https?:\/\/)?[^\/]+\/(.+)$";
                Match match = Regex.Match(course.ImageUrl, pattern);

                if (match.Success)
                {
                    course.ImageUrl = match.Groups[1].Value;
                }

                course.Specialities = requestDto.SpecialityIds.Select(x => new CourseSpeciality { SpecialityId = x }).ToList();
                course.Prerequisites = requestDto.PrerequisiteCourseIds.Select(x => new PrerequisiteCourse { PrerequisiteCourseId = x }).ToList();
                course.Comunity = new Comunity();
                if (requestDto.IsPublished)
                {
                    course.PublishedAt = DateTime.Now;
                }

                var userId = _sessionService.CurrentUser?.Id;

                if (userId == null)
                {
                    throw new Exception("You have not loged in");
                }

                course.OwnerId = userId.Value;

                _unitOfWork.Courses.Add(course);
                _unitOfWork.SaveChanges();
                transaction.Commit();

                return _mapper.Map<CourseDto>(course);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
        public async Task<CourseDto> UpdateCourse(Course_UpdateRequestDto requestDto, int id)
        {
            var course = await _unitOfWork.Courses.GetAll()
                .Include(c => c.Specialities)
                .Include(c => c.Prerequisites)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
            {
                throw new Exception("Course not found");
            }
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                string pattern = @"^(?:https?:\/\/)?[^\/]+\/(.+)$";
                Match match = Regex.Match(course.ImageUrl, pattern);

                if (match.Success)
                {
                    course.ImageUrl = match.Groups[1].Value;
                }

                if (requestDto.IsPublished != null && requestDto.IsPublished == true)
                {
                    course.PublishedAt = DateTime.Now;
                }

                if (requestDto.SpecialityIds != null)
                {
                    // Remove all existing specialities
                    _unitOfWork.CourseSpecialities.RemoveRange(course.Specialities);
                    // Add new specialities
                    course.Specialities = requestDto.SpecialityIds.Select(x => new CourseSpeciality { SpecialityId = x }).ToList();
                }

                if (requestDto.PrerequisiteCourseIds != null)
                {
                    // Remove all existing prerequisites
                    _unitOfWork.PrerequisiteCourses.RemoveRange(course.Prerequisites);
                    // Add new prerequisites
                    course.Prerequisites = requestDto.PrerequisiteCourseIds.Select(x => new PrerequisiteCourse { PrerequisiteCourseId = x }).ToList();
                }

                course = _mapper.Map(requestDto, course);

                _unitOfWork.SaveChanges();
                transaction.Commit();

                return _mapper.Map<CourseDto>(course);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
        public async Task<CourseDto> GetCourseById(Course_GetByIdRequestDto request, int id)
        {

            var query = _unitOfWork.Courses.GetAll()
                .Include(c => c.Owner)
                .Include(c => c.Prerequisites)
                .Include(c => c.Specialities)
                    .ThenInclude(cs => cs.Speciality)
                .Include(c => c.CourseGroup)
                .Include(c => c.Topics)
                    .ThenInclude(t => t.LearningObjects)
                .Where(x => x.Id == id);

            var learnerId = _sessionService.CurrentUser?.Learner?.Id;

            if (learnerId != null)
            {
                query = query
                    .Include(c => c.Topics)
                        .ThenInclude(t => t.TopicLearningPathOrders.Where(o => o.LearnerId == learnerId))
                    .Include(c => c.Topics)
                        .ThenInclude(t => t.LearningObjects)
                            .ThenInclude(lo => lo.LearningObjectLearningPathOrders.Where(o => o.LearnerId == learnerId));

            }
            if (request.IsGetFullDetail)
            {
                query = query
                    .Include(c => c.Topics)
                        .ThenInclude(t => t.LearningObjects)
                            .ThenInclude(lo => lo.Video)
                   .Include(c => c.Topics)
                            .ThenInclude(t => t.LearningObjects)
                                .ThenInclude(lo => lo.Quiz)
                                    .ThenInclude(q => q.Questions)
                                        .ThenInclude(q => q.Answers);


            }

            var course = await query.FirstOrDefaultAsync();

            if (course == null)
            {
                throw new Exception("Course not found");
            }

            var result = _mapper.Map<CourseDto>(course);
            return result;

        }
        public async Task<Course_GetResponseDto> GetPaginatedData(Course_GetRequestDto requestDto, int? offset, int? limit, string? cursor)
        {
            var query = _unitOfWork.Courses.GetAll();

            query = query
                .Include(x => x.Specialities)
                .Include(x => x.Topics)
                    .ThenInclude(t => t.LearningObjects)
                .Include(c => c.Owner);

            if (requestDto.SpecialityIds.Count > 0)
            {
                query = query
                    .Where(x => x.Specialities.Any(s => requestDto.SpecialityIds.Contains(s.SpecialityId)))
                    .Where(x => x.IsPublished);
            }

            if (offset != null)
            {
                query = query.Skip(offset.Value);
            }

            if (limit != null)
            {
                query = query.Take(limit.Value);
            }

            var courses = await query.ToListAsync();

            var courseDtos = _mapper.Map<List<CourseDto>>(courses);

            foreach (var c in courseDtos)
            {
                var los = c.Topics.SelectMany(t => t.LearningObjects).ToList();
                var count = los.Count;
                c.DifficultyLevel = count > 0 ? los.Average(x => x.Difficulty) : 0;
                c.Topics = new List<RecommendTopicDto>();
            }
            return new Course_GetResponseDto
            {
                Courses = courseDtos
            };
        }
        public async Task<int> GetTotalCount()
        {
            var courses = _unitOfWork.Courses.GetAll();
            return await courses.CountAsync();
        }

        public async Task<ComunityDto> GetComnunity(int courseId)
        {
            var comunity = await _unitOfWork.Comunities.GetAll()
                .Include(c => c.Discussions)
                    .ThenInclude(d => d.Owner)
                .FirstOrDefaultAsync(x => x.CourseId == courseId);

            if (comunity == null)
            {
                throw new Exception("Comunity not found");
            }

            return _mapper.Map<ComunityDto>(comunity);
        }

        public async Task<Course_GetResponseDto> GetRecentLearningCourses(int limit = 3)
        {
            var user = _sessionService.CurrentUser;
            if (user == null)
            {
                throw new Exception("You have not loged in");
            }

            var learner = user.Learner;
            if (learner == null)
            {
                throw new Exception("You are not a learner");
            }

            var query = _unitOfWork.Courses.GetAll();

            query = query
                .Include(x => x.Specialities)
                .Include(c => c.Owner)
                .Where(c => c.Specialities.Any(s => s.SpecialityId == learner.SpecialityId))
                .Where(x => x.Topics.Any(t => t.LearningObjects.Any(lo => lo.LearnerLogs.Any(ll => ll.LearnerId == learner.Id))))
                .OrderByDescending(x => x.Topics.SelectMany(t => t.LearningObjects).SelectMany(lo => lo.LearnerLogs)
                    .Where(ll => ll.LearnerId == learner.Id)
                    .Max(ll => ll.UpdatedAt != null ? ll.UpdatedAt : ll.CreatedAt))
                .Take(limit);

            var courses = await query.ToListAsync();

            var courseDtos = _mapper.ProjectTo<CourseDto>(courses.AsQueryable()).ToList();

            return new Course_GetResponseDto
            {
                Courses = courseDtos
            };
        }

        public async Task<Course_GetResponseDto> GetPopularCourse(int limit = 5)
        {
            var user = _sessionService.CurrentUser;
            if (user == null)
            {
                throw new Exception("You have not loged in");
            }

            var learner = user.Learner;
            if (learner == null)
            {
                throw new Exception("You are not a learner");
            }

            var query = _unitOfWork.Courses.GetAll();

            query = query
                .Include(x => x.Specialities)
                .Include(c => c.Owner)
                .Where(c => c.Specialities.Any(s => s.SpecialityId == learner.SpecialityId))
                .OrderByDescending(x => x.Topics.SelectMany(t => t.LearningObjects).SelectMany(lo => lo.LearnerLogs).Select(ll => ll.LearnerId).Distinct().Count())
                .Take(limit);

            var courses = await query.ToListAsync();

            var courseDtos = _mapper.ProjectTo<CourseDto>(courses.AsQueryable()).ToList();

            return new Course_GetResponseDto
            {
                Courses = courseDtos
            };
        }
    }
}

