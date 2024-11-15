using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.DataAccess.Abstract;
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
            course.Specialities = requestDto.SpecialityIds.Select(x => new CourseSpeciality { SpecialityId = x }).ToList();
            if (requestDto.IsPublished)
            {
                course.PublishedAt = DateTime.Now;
            }
            course.OwnerId = _sessionService.CurrentUser.Id;

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
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
            var course = await _unitOfWork.Courses.GetAll().Include(c => c.Specialities).FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
            {
                throw new Exception("Course not found");
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

            course = _mapper.Map(requestDto, course);

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
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
        public async Task<CourseDto> GetCourseById(Course_GetByIdRequestDto requestDto, int id)
        {
            var course = await _unitOfWork.Courses.GetAll()
                .Include(c => c.Topics)
                        .ThenInclude(t => t.LearningObjects)
                            .ThenInclude(lo => lo.Video)
               .Include(c => c.Topics)
                        .ThenInclude(t => t.LearningObjects)
                            .ThenInclude(lo => lo.Quiz)
                                .ThenInclude(q => q.Questions)
                                    .ThenInclude(q => q.Answers)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new Exception("Course not found");
            }

            return _mapper.Map<CourseDto>(course);

        }
        public async Task<Course_GetResponseDto> GetPaginatedData(Course_GetRequestDto requestDto, int? offset, int? limit, string? cursor)
        {
            var query = _unitOfWork.Courses.GetAll();

            query = query.Include(x => x.Specialities).Where(x => x.IsPublished);

            if (requestDto.SpecialityIds.Count > 0)
            {
                query = query.Where(x => x.Specialities.Any(s => requestDto.SpecialityIds.Contains(s.SpecialityId)));
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

