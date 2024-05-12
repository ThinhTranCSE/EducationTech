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
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to submit quizzes");
            }
            var quizQuery = await _quizRepository.Get(); 
            quizQuery = quizQuery
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Lesson)
                    .ThenInclude(q => q.CourseSection)
                            .ThenInclude(q => q.Course)
                                .ThenInclude(q => q.LearnerCourses)
                .Where(q => q.Id == requestDto.QuizId)
                .Where(q => q.Lesson.CourseSection.Course.LearnerCourses.Any(x => x.LearnerId == currentUser.Id));
                
            var quiz = await quizQuery.FirstOrDefaultAsync();
            if (quiz == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Quiz not found or you do not have permission to submit this quiz");
            }
            var answerBaseIdsSet = new HashSet<int>(quiz.Questions.SelectMany(question => question.Answers).Select(a => a.Id));
            if(requestDto.AnswerIds.Any(a => !answerBaseIdsSet.Contains(a)))
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Invalid answer id");
            }
            var insertedAnswerUserTasks = requestDto.AnswerIds.Select(answerId => _answerUserRepository.Insert(new AnswerUser
            {
                UserId = currentUser.Id,
                AnswerId = answerId
            })).ToArray();

            Task.WaitAll(insertedAnswerUserTasks);

            _transactionManager.SaveChanges();


            var correctAnswerIds = quiz.Questions.SelectMany(question => question.Answers).Where(a => a.IsCorrect).Select(a => a.Id).ToArray();
            return new Lesson_ValidateQuizResponseDto
            {
                CorrectAnswerIds = correctAnswerIds
            };
        }
    }
}
