using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

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
                course.Specialities = requestDto.SpecialityIds.Select(x => new CourseSpeciality { SpecialityId = x }).ToList();
                course.Prerequisites = requestDto.PrerequisiteCourseIds.Select(x => new PrerequisiteCourse { PrerequisiteCourseId = x }).ToList();
                course.Comunity = new Comunity();
                if (requestDto.IsPublished)
                {
                    course.PublishedAt = DateTime.Now;
                }
                course.OwnerId = _sessionService.CurrentUser.Id;

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
    }
}

