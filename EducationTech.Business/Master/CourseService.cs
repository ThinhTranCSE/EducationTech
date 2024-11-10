using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.DataAccess.Abstract;
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
            throw new NotImplementedException();
        }
        public async Task<CourseDto> UpdateCourse(Course_UpdateRequestDto requestDto, int id)
        {
            throw new NotImplementedException();
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

