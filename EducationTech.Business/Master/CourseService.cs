using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.Business.Shared.Types;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            var currentUser = _sessionService.CurrentUser;
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create course");
            }
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var createdCourse = _mapper.Map<Course>(requestDto);
                createdCourse.OwnerId = currentUser.Id;

                _unitOfWork.Courses.Add(createdCourse);
                _unitOfWork.SaveChanges();

                var categoryQuery = _unitOfWork.Categories.GetAll();
                var categories = await categoryQuery.Where(x => requestDto.CategoryIds.Contains(x.Id)).ToListAsync();

                var courseCategories = categories.Select(x => new CourseCategory
                {
                    CategoryId = x.Id,
                    CourseId = createdCourse.Id
                }).ToList();

                _unitOfWork.CourseCategories.AddRange(courseCategories);
                _unitOfWork.SaveChanges();

                _unitOfWork.LearnerCourses.Add(new LearnerCourse
                {
                    CourseId = createdCourse.Id,
                    LearnerId = currentUser.Id
                });
                _unitOfWork.SaveChanges();

                transaction.Commit();

                return _mapper.Map<CourseDto>(createdCourse);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public async Task<CourseDto> UpdateCourse(Course_UpdateRequestDto requestDto, int id)
        {
            var currentUser = _sessionService.CurrentUser;
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update course");
            }
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var courseQuery = _unitOfWork.Courses.GetAll();
                courseQuery = courseQuery
                    .Include(x => x.Owner)
                    .Include(x => x.CourseCategories)
                    .Where(x => x.Id == id);

                var course = await courseQuery.FirstOrDefaultAsync();
                if (course == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Course not found");
                }
                if (course.OwnerId != currentUser.Id)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to update this course");
                }
                if (requestDto.Description != null)
                {
                    course.Description = requestDto.Description;
                }
                if (requestDto.Title != null)
                {
                    course.Title = requestDto.Title;
                }
                //if (requestDto.Price != null)
                //{
                //    course.Price = requestDto.Price.Value;
                //}
                if (requestDto.ImageUrl != null)
                {
                    course.ImageUrl = requestDto.ImageUrl;
                }
                if (requestDto.IsArchived != null)
                {
                    currentUser = await _unitOfWork.Users.GetAll()
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
                if (requestDto.IsPublished != null)
                {
                    if (course.OwnerId != currentUser.Id)
                    {
                        throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to publish course");
                    }

                    course.IsPublished = requestDto.IsPublished.Value;
                }
                if (requestDto.CategoryIds != null)
                {
                    var categoryQuery = _unitOfWork.Categories.GetAll();
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
                    _unitOfWork.CourseCategories.RemoveRange(deletedCourseCategories);

                    //insert new categories to course
                    var insertedCategories = await categoryQuery.Where(c => insertedCategoryIds.Contains(c.Id)).ToListAsync();
                    var insertedCourseCategories = insertedCategories.Select(ic => new CourseCategory
                    {
                        CategoryId = ic.Id,
                        CourseId = course.Id
                    }).ToList();

                    _unitOfWork.CourseCategories.AddRange(insertedCourseCategories);

                    _unitOfWork.SaveChanges();
                }

                _unitOfWork.SaveChanges();

                transaction.Commit();

                course = await courseQuery.FirstOrDefaultAsync();

                return _mapper.Map<CourseDto>(course);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public async Task<CourseDto> GetCourseById(Course_GetByIdRequestDto requestDto, int id)
        {
            var currentUser = _sessionService.CurrentUser;
            var query = _unitOfWork.Courses.GetAll();
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
                bool flag = false;
                if (currentUser == null)
                {
                    //throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get course detail");
                    flag = true;
                }
                var leanerCourse = !flag ? _unitOfWork.LearnerCourses.Find(lc => lc.CourseId == id && lc.LearnerId == currentUser.Id).FirstOrDefault() : null;
                var c = _unitOfWork.Courses.Find(x => x.Id == id).FirstOrDefault();
                if (leanerCourse == null)
                {
                    //throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to view this course detail");
                    flag = true;
                }
                if (currentUser != null && c.OwnerId == currentUser.Id)
                {
                    flag = false;
                }
                query = query
                    .Include(x => x.CourseSections)
                        .ThenInclude(x => x.Lessons)
                            .ThenInclude(x => x.Quiz)
                                .ThenInclude(x => x.Questions)
                                    .ThenInclude(x => x.Answers)
                    .Include(x => x.CourseSections)
                        .ThenInclude(x => x.Lessons)
                            .ThenInclude(x => x.Video);

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
            if (course.OwnerId != currentUser?.Id && (course.IsArchived || !course.IsPublished))
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to view this course detail");
            }

            var courseDto = _mapper.Map<CourseDto>(course);

            if (requestDto.IsIncludeRate)
            {
                bool isLearnerCouseEmpty = course.LearnerCourses.Count() == 0;
                courseDto.Rate = isLearnerCouseEmpty ? 0 : course.LearnerCourses.Average(x => x.Rate);
            }

            return courseDto;
        }
        public async Task<Course_GetResponseDto> GetPaginatedData(Course_GetRequestDto requestDto, int? offset, int? limit, string? cursor)
        {
            var currentUser = _sessionService.CurrentUser;
            var query = _unitOfWork.Courses.GetAll();

            switch (requestDto.OrderBy)
            {
                case CourseOrderByType.Id:
                    query = query.OrderBy(x => x.Id);
                    break;
                case CourseOrderByType.PublishedAt:
                    query = query.OrderBy(x => x.PublishedAt);
                    break;
                case CourseOrderByType.Rate:
                    query = query
                        .Include(x => x.LearnerCourses)
                        .OrderBy(x => x.LearnerCourses.Average(y => y.Rate));
                    break;
                default:
                    query = query.OrderBy(x => x.PublishedAt);
                    break;
            }
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

            if (requestDto.IsExcludeBought)
            {
                if (currentUser != null)
                {
                    query = query
                        .Include(x => x.LearnerCourses)
                        .Where(x => x.LearnerCourses.All(y => y.LearnerId != currentUser.Id));
                }
            }

            if (!requestDto.IsIncludeArchived)
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
            if (requestDto.IsIncludeRate)
            {
                query = query.Include(x => x.LearnerCourses);
            }

            if (requestDto.CreatedByCurrentUser)
            {
                if (currentUser == null)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get created courses");
                }
                query = query.Where(x => x.OwnerId == currentUser.Id)
                    .Where(x => !x.IsArchived);
            }

            if (offset.HasValue && limit.HasValue)
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
                    var isLearnerCouseEmpty = courses.ElementAt(i).LearnerCourses.Count() == 0;
                    courseDtos.ElementAt(i).Rate = isLearnerCouseEmpty ? 0 : courses.ElementAt(i).LearnerCourses.Average(x => x.Rate);
                }
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

        public async Task<CourseDto> BuyCourse(Course_BuyRequestDto requestDto, int id)
        {
            var currentUser = _sessionService.CurrentUser;
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to buy course");
            }

            var courseQuery = _unitOfWork.Courses.GetAll();
            courseQuery = courseQuery
                .Include(x => x.LearnerCourses)
                .Where(x => x.Id == id);
            var course = await courseQuery.FirstOrDefaultAsync();

            if (course == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course not found");
            }

            if (course.OwnerId == currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "You cant buy your own course");
            }

            var learnerCourse = _unitOfWork.LearnerCourses.Find(x => x.CourseId == id && x.LearnerId == currentUser.Id).FirstOrDefault();
            if (learnerCourse != null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "You already bought this course");
            }

            learnerCourse = new LearnerCourse
            {
                CourseId = id,
                LearnerId = currentUser.Id,
            };

            _unitOfWork.LearnerCourses.Add(learnerCourse);
            _unitOfWork.SaveChanges();

            return _mapper.Map<CourseDto>(course);
        }
    }
}

