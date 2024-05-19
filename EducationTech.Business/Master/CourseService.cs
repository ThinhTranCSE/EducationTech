using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.Business.Shared.Types;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.Shared.Enums;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICourseCategoryRepository _courseCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public CourseService(
            ITransactionManager transactionManager, 
            ICourseRepository courseRepository, 
            ILearnerCourseRepository learnerCourseRepository, 
            ICategoryRepository categoryRepository,
            ICourseCategoryRepository courseCategoryRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _transactionManager = transactionManager;
            _courseRepository = courseRepository;
            _learnerCourseRepository = learnerCourseRepository;
            _categoryRepository = categoryRepository;
            _courseCategoryRepository = courseCategoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CourseDto> CreateCourse(Course_CreateRequestDto requestDto, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create course");
            }
            var createdCourse = _mapper.Map<Course>(requestDto);
            createdCourse.OwnerId = currentUser.Id;

            await _courseRepository.Insert(createdCourse, true);

            var categoryQuery = await _categoryRepository.Get();
            var categories = await categoryQuery.Where(x => requestDto.CategoryIds.Contains(x.Id)).ToListAsync();

            var courseCategories = categories.Select(x => new CourseCategory
            {
                CategoryId = x.Id,
                CourseId = createdCourse.Id
            }).ToList();

            await _courseCategoryRepository.Insert(courseCategories, true);
            
            return _mapper.Map<CourseDto>(createdCourse);
        }
        public async Task<CourseDto> UpdateCourse(Course_UpdateRequestDto requestDto, int id, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update course");
            }
            var courseQuery = await _courseRepository.Get();
            courseQuery = courseQuery
                .Include(x => x.Owner)
                .Include(x => x.CourseCategories)
                .Where(x => x.Id == id);

            var course = await courseQuery.FirstOrDefaultAsync();
            if(course == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course not found");
            }
            if(course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to update this course");
            }
            if(requestDto.Description != null)
            {
                course.Description = requestDto.Description;
            }
            if(requestDto.Title != null)
            {
                course.Title = requestDto.Title;
            }
            if(requestDto.Price != null)
            {
                course.Price = requestDto.Price.Value;
            }
            if(requestDto.ImageUrl != null)
            {
                course.ImageUrl = requestDto.ImageUrl;
            }
            if(requestDto.IsArchived != null)
            {
                currentUser = await (await _userRepository.Get())
                    .Where(u => u.Id == currentUser.Id)
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                            .ThenInclude(r => r.RolePermissions)
                                .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync();

                var hasPermission = currentUser!.UserRoles
                    .Select(ur => ur.Role)
                    .SelectMany(r => r.RolePermissions)
                    .Select(rp => rp.Permission)
                    .Any(p => p.Name == nameof(PermissionType.ArchivedCourse));

                if (!hasPermission)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to publish course");
                }
                course.IsArchived = requestDto.IsArchived.Value;
            }
            if(requestDto.IsPublished != null)
            {
                if(course.OwnerId != currentUser.Id)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to publish course");
                }
                
                course.IsPublished = requestDto.IsPublished.Value;
            }
            if(requestDto.CategoryIds != null)
            {
                var categoryQuery = await _categoryRepository.Get();
                var currentCourseCategories = await categoryQuery
                    .Include(c => c.CourseCategories)
                    .Where(c => c.CourseCategories.Any(cc => cc.CourseId == course.Id))
                    .ToListAsync();
                //get current categor of course
                var currentCategoryIds = currentCourseCategories.SelectMany(c => c.CourseCategories).Select(cc => cc.CategoryId);

                //get inserted and deleted category ids
                var insertedCategoryIds = requestDto.CategoryIds.Except(currentCategoryIds).ToHashSet();
                var deletedCategoryIds = currentCategoryIds.Except(requestDto.CategoryIds).ToHashSet();

                //delete removed categories of course
                var deletedCourseCategories = course.CourseCategories.Where(cc => deletedCategoryIds.Contains(cc.CategoryId)).ToList();
                deletedCourseCategories.ForEach(cc => _courseCategoryRepository.Delete(cc).GetAwaiter().GetResult());

                //insert new categories to course
                var insertedCategories = await categoryQuery.Where(c => insertedCategoryIds.Contains(c.Id)).ToListAsync();
                var insertedCourseCategories = insertedCategories.Select(ic => new CourseCategory
                {
                    CategoryId = ic.Id,
                    CourseId = course.Id
                }).ToList();

                await _courseCategoryRepository.Insert(insertedCourseCategories, true);
            }

            await _courseRepository.Update(course, true);
            course = await courseQuery.FirstOrDefaultAsync();

            return _mapper.Map<CourseDto>(course);

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
                var leanerCourse = !flag ? await _learnerCourseRepository.GetSingle(lc => lc.CourseId == id && lc.LearnerId == currentUser.Id, false) : null;
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
                .Include(x => x.CourseCategories)
                    .ThenInclude(x => x.Category);


            var course = await query.FirstOrDefaultAsync();
            if (course == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course not found");
            }
            if(course.OwnerId != currentUser?.Id && (course.IsArchived || !course.IsPublished))
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to view this course detail");
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

            if (!requestDto.IsIncludeNotPublished && !requestDto.CreatedByCurrentUser)
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

            if(requestDto.CreatedByCurrentUser)
            {
                if(currentUser == null)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get created courses");
                }
                query = query.Where(x => x.OwnerId == currentUser.Id)
                    .Where(x => !x.IsArchived);
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

