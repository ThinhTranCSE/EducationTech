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
                course.Specialities.Clear();
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
            throw new NotImplementedException();
        }
        public async Task<Course_GetResponseDto> GetPaginatedData(Course_GetRequestDto requestDto, int? offset, int? limit, string? cursor)
        {
            throw new NotImplementedException();
        }
        public async Task<int> GetTotalCount()
        {
            var courses = _unitOfWork.Courses.GetAll();
            return await courses.CountAsync();
        }

    }
}

