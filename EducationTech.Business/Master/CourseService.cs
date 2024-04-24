using AutoMapper;
using EducationTech.Business.Abstract;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master
{
    public class CourseService : ICourseService, IPagination<Course_GetRequestDto, Course_GetResponseDto>
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        public CourseService(ITransactionManager transactionManager, ICourseRepository courseRepository, IMapper mapper)
        {
            _transactionManager = transactionManager;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }   


        public async Task<Course_GetResponseDto> GetPaginatedData(Course_GetRequestDto requestDto, int? offset, int? limit, string? cursor)
        {
            var query = await _courseRepository.Get();

            if(offset.HasValue && limit.HasValue)
            {
                query = query
                    .OrderBy(query => query.Id)
                    .Skip(offset.Value)
                    .Take(limit.Value);
            }
            var courseDtos = _mapper.ProjectTo<CourseDto>(query);
            return new Course_GetResponseDto
            {
                Courses = courseDtos.ToList()
            };
        }

        public async Task<int> GetTotalCount()
        {
            var courses = await _courseRepository.Get();
            return await courses.CountAsync();
        }
    }
}
