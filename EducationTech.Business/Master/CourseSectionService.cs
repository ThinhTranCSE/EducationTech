using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.CourseSections;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Master;

public class CourseSectionService : ICourseSectionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;

    public CourseSectionService(IUnitOfWork unitOfWork, ISessionService sessionService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
    }


    public async Task<CourseSectionDto> CreateCourseSection(CourseSection_CreateRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create course section");
        }
        var courseQuery = _unitOfWork.Courses.GetAll();
        courseQuery = courseQuery
            .Include(c => c.Owner)
            .Where(x => x.Id == requestDto.CourseId);
        var course = await courseQuery.FirstOrDefaultAsync();

        if (course == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Course not found");
        }
        if (course.OwnerId != currentUser.Id)
        {
            throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create course section for this course");
        }

        var createdCourseSection = _mapper.Map<CourseSection>(requestDto);

        _unitOfWork.CourseSections.Add(createdCourseSection);
        _unitOfWork.SaveChanges();

        return _mapper.Map<CourseSectionDto>(createdCourseSection);
    }

    public async Task<CourseSectionDto> UpdateCourseSection(int id, CourseSection_UpdateRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update course section");
        }

        var courseSectionQuery = _unitOfWork.CourseSections.GetAll();
        courseSectionQuery = courseSectionQuery
            .Include(c => c.Course)
            .ThenInclude(c => c.Owner)
            .Where(x => x.Id == id);

        var courseSection = await courseSectionQuery.FirstOrDefaultAsync();

        if (courseSection == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Course section not found");
        }
        if (courseSection.Course.OwnerId != currentUser.Id)
        {
            throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update course section for this course");
        }

        if (requestDto.Title != null)
        {
            courseSection.Title = requestDto.Title;
        }
        if (requestDto.Order != null)
        {
            courseSection.Order = requestDto.Order.Value;
        }

        _unitOfWork.SaveChanges();

        return _mapper.Map<CourseSectionDto>(courseSection);
    }
}
