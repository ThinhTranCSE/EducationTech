using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
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
        private readonly IQuizRepository _quizRepository;
        private readonly IAnswerRepositoy _answerRepositoy;
        private readonly IAnswerUserRepository _answerUserRepository;
        private readonly ILearnerCourseRepository _learnerCourseRepository;
        private readonly IMapper _mapper;

        public LessonService(
            ITransactionManager transactionManager, 
            ILessonRepository lessonRepository, 
            IQuizRepository quizRepository, 
            IAnswerRepositoy answerRepositoy, 
            IAnswerUserRepository answerUserRepository,
            ILearnerCourseRepository learnerCourseRepository, 
            IMapper mapper)
        {
            _transactionManager = transactionManager;
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _answerRepositoy = answerRepositoy;
            _answerUserRepository = answerUserRepository;
            _learnerCourseRepository = learnerCourseRepository;
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

        public async Task<Lesson_ValidateQuizResponseDto> SubmitAnswers(Lesson_ValidateQuizRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to validate quizzes");
            }
            HashSet<int> answerIdsSet = new HashSet<int>(requestDto.AnswerIds);
            var quizQuery = await _answerRepositoy.Get(); 
            quizQuery = quizQuery
                .Include(x => x.Question)
                    .ThenInclude(x => x.Quiz)
                        .ThenInclude(x => x.Lesson)
                            .ThenInclude(x => x.CourseSection)
                                .ThenInclude(x => x.Course)
                                    .ThenInclude(x => x.LearnerCourses)
                .Where(x => x.Question.Quiz.Lesson.CourseSection.Course.LearnerCourses.Any(y => y.LearnerId == currentUser.Id))
                .Where(x => answerIdsSet.Contains(x.Id));
            
            var answers = await quizQuery.ToListAsync();
            if (answers.Count != answerIdsSet.Count)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Invalid answer ids");
            }
            var answerUsers = answers.Select(x => new AnswerUser
            {
                AnswerId = x.Id,
                UserId = currentUser.Id
            });
            await _answerUserRepository.Insert(answerUsers);
            _transactionManager.SaveChanges();

            var correctAnswerIds = answers.Where(x => x.IsCorrect).Select(x => x.Id).ToList();
            return new Lesson_ValidateQuizResponseDto
            {
                CorrectAnswerIds = correctAnswerIds
            };
        }
    }
}
