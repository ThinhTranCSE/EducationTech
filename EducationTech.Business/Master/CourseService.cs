using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.Business.Shared.Types;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master
{
    public class CourseService : ICourseService, IPagination<Course_GetRequestDto, Course_GetResponseDto>
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ICourseRepository _courseRepository;
        private readonly ILearnerCourseRepository _learnerCourseRepository;
        private readonly IMapper _mapper;
        public CourseService(ITransactionManager transactionManager, ICourseRepository courseRepository, ILearnerCourseRepository learnerCourseRepository, IMapper mapper)
        {
            _transactionManager = transactionManager;
            _courseRepository = courseRepository;
            _learnerCourseRepository = learnerCourseRepository;
            _mapper = mapper;
        }

        public async Task<CourseDto> GetCourseById(Course_GetByIdRequestDto requestDto, int id, User? currentUser)
        {
            var query = await _courseRepository.Get();
            if (requestDto.BelongToCurrentUser)
            {
                if (currentUser == null)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get buyed courses");
                }
                query = query
                    .Include(x => x.LearnerCourses)
                    .Where(x => x.LearnerCourses.Any(y => y.LearnerId == currentUser.Id));
            }
            if (requestDto.IsGetDetail)
            {
                var beforeChangeQuery = query;
                var flag = false;
                if (currentUser == null)
                {
                    //throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get course detail");
                    flag = true;
                }
                var leanerCourse = await _learnerCourseRepository.GetSingle(lc => lc.CourseId == id && lc.LearnerId == currentUser.Id, false);
                if (leanerCourse == null)
                {
                    //throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to view this course detail");
                    flag = true;
                }
                query = query
                    .Include(x => x.CourseSections)
                        .ThenInclude(x => x.Lessons);
                
                if (flag) { query = beforeChangeQuery; }
            }

            if (requestDto.IsIncludeRate)
            {
                query = query.Include(x => x.LearnerCourses);
            }

            query = query
                .Where(x => x.Id == id)
                .Include(x => x.Owner)
                .Where(x => !x.IsArchived)
                .Where(x => x.IsPublished);

            var course = await query.FirstOrDefaultAsync();
            if (course == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course not found");
            }
            var courseDto = _mapper.Map<CourseDto>(course);

            if (requestDto.IsIncludeRate)
            {
                courseDto.Rate = course.LearnerCourses.Average(x => x.Rate);
            }

            return courseDto;
        }

        public async Task<Course_GetResponseDto> GetPaginatedData(Course_GetRequestDto requestDto, int? offset, int? limit, string? cursor, User? currentUser)
        {
            var query = await _courseRepository.Get();

            switch (requestDto.OrderBy)
            {
                case CourseOrderByType.Id:
                    query = query.OrderBy(x => x.Id);
                    break;
                case CourseOrderByType.CreatedAt:
                    query = query.OrderBy(x => x.CreatedAt);
                    break;
                case CourseOrderByType.Rate:
                    query = query
                        .Include(x => x.LearnerCourses)
                        .OrderBy(x => x.LearnerCourses.Average(y => y.Rate));
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }
            if(requestDto.BelongToCurrentUser)
            {
                if(currentUser == null)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get buyed courses");
                }
                query = query
                    .Include(x => x.LearnerCourses)
                    .Where(x => x.LearnerCourses.Any(y => y.LearnerId == currentUser.Id));
            }
            if(!requestDto.IsIncludeArchived)
            {
                query = query.Where(x => !x.IsArchived);
            }

            if (!requestDto.IsIncludeNotPublished)
            {
                query = query.Where(x => x.IsPublished);
            }

            if (requestDto.IsIncludeOwner)
            {
                query = query.Include(x => x.Owner);
            }
            if(requestDto.IsIncludeRate)
            {
                query = query.Include(x => x.LearnerCourses);
            }
            if(offset.HasValue && limit.HasValue)
            {
                query = query
                    .Skip(offset.Value)
                    .Take(limit.Value);
            }
            var courses = await query.ToListAsync();
            var courseDtos = _mapper.ProjectTo<CourseDto>(courses.AsQueryable()).ToList();

            if (requestDto.IsIncludeRate)
            {
                int courseCount = courseDtos.Count();
                for (int i = 0; i < courseCount; i++)
                {
                    courseDtos.ElementAt(i).Rate = courses.ElementAt(i).LearnerCourses.Average(x => x.Rate);
                }
            }

            return new Course_GetResponseDto
            {
                Courses = courseDtos
            };
        }


        public async Task<int> GetTotalCount()
        {
            var courses = await _courseRepository.Get();
            return await courses.CountAsync();
        }
    }
}
