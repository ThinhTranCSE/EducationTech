using AutoMapper;
using EducationTech.Business.Shared.DTOs.Masters.CourseSections;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public class CourseSectionService : ICourseSectionService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseSectionRepository _courseSectionRepository;
        private readonly IMapper _mapper;

        public CourseSectionService(ICourseRepository courseRepository, ICourseSectionRepository courseSectionRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _courseSectionRepository = courseSectionRepository;
            _mapper = mapper;
        }


        public async Task<CourseSectionDto> CreateCourseSection(CourseSection_CreateRequestDto requestDto, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create course section");
            }
            var courseQuery = await _courseRepository.Get();
            courseQuery = courseQuery
                .Include(c => c.Owner)
                .Where(x => x.Id == requestDto.CourseId);
            var course = await courseQuery.FirstOrDefaultAsync();

            if(course == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course not found");
            }
            if(course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create course section for this course");
            }

            var createdCourseSection = _mapper.Map<CourseSection>(requestDto);

            await _courseSectionRepository.Insert(createdCourseSection, true);

            return _mapper.Map<CourseSectionDto>(createdCourseSection);
        }

        public async Task<CourseSectionDto> UpdateCourseSection(int id, CourseSection_UpdateRequestDto requestDto, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update course section");
            }

            var courseSectionQuery = await _courseSectionRepository.Get();
            courseSectionQuery = courseSectionQuery
                .Include(c => c.Course)
                .ThenInclude(c => c.Owner)
                .Where(x => x.Id == id);

            var courseSection = await courseSectionQuery.FirstOrDefaultAsync();

            if(courseSection == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course section not found");
            }
            if(courseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update course section for this course");
            }

            if(requestDto.Title != null)
            {
                courseSection.Title = requestDto.Title;
            }
            if(requestDto.Order != null)
            {
                courseSection.Order = requestDto.Order.Value;
            }

            await _courseSectionRepository.Update(courseSection, true);
            return _mapper.Map<CourseSectionDto>(courseSection);
        }
    }
}
