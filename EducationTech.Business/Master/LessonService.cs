using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Business.Shared.Exceptions.Http;
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
    public class LessonService : ILessonService
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILearnerCourseRepository _learnerCourseRepository;
        private readonly IMapper _mapper;

        public LessonService(ITransactionManager transactionManager, ILessonRepository lessonRepository, IMapper mapper)
        {
            _transactionManager = transactionManager;
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }
        public async Task<LessonDto> GetLessonById(int id, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get lesson detail");
            }

            var query = await _lessonRepository.Get();
            query = query
                .Where(x => x.Id == id)
                .Include(x => x.Video)
                .Include(x => x.Quiz)
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.Answers)
                .Include(x => x.CourseSection)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.LearnerCourses);
            var lesson = await query.FirstOrDefaultAsync();
            if (lesson == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
            }
            if(lesson.CourseSection.Course.LearnerCourses.All(x => x.LearnerId != currentUser.Id))
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to view this lesson detail");
            }

            return _mapper.Map<LessonDto>(lesson);
        }
    }
}
