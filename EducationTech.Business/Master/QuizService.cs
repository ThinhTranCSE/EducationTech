using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
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
    public class QuizService : IQuizService
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly ILessonRepository _lessonRepository;

        public QuizService(
            ITransactionManager transactionManager, 
            IMapper mapper,
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ILessonRepository lessonRepository
            )
        {
            _transactionManager = transactionManager;
            _mapper = mapper;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _lessonRepository = lessonRepository;

        }

        public async Task<QuizDto> CreateQuiz(Quiz_CreateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create quiz");
            }

            var lessonQuery = await _lessonRepository.Get();
            lessonQuery = lessonQuery
                .Include(l => l.CourseSection)
                    .ThenInclude(l => l.Course)
                        .ThenInclude(l => l.Owner)
                .Where(l => l.Id == requestDto.LessonId);

            var lesson = await lessonQuery.FirstOrDefaultAsync();

            if (lesson == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
            }

            if (lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create quiz for this lesson");
            }

            var createdQuiz = _mapper.Map<Quiz>(requestDto);

            await _quizRepository.Insert(createdQuiz, true);

            var quiz = await  (await _quizRepository.Get())
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Where(q => q.Id == createdQuiz.Id)
                .FirstOrDefaultAsync();

            return _mapper.Map<QuizDto>(quiz);
        }

        //public async Task<bool> DeleteQuiz(int id, User? currentUser)
        //{
        //    if (currentUser == null)
        //    {
        //        throw new HttpException(HttpStatusCode.Unauthorized, "Please login to delete quiz");
        //    }

        //    var quizQuery = await _quizRepository.Get();
        //    quizQuery = quizQuery
        //        .Include(q => q.Lesson)
        //            .ThenInclude(q => q.CourseSection)
        //                .ThenInclude(q => q.Course)
        //                    .ThenInclude(q => q.Owner)
        //        .Where(q => q.Id == id);

        //    var quiz = await quizQuery.FirstOrDefaultAsync();

        //    if (quiz == null)
        //    {
        //        throw new HttpException(HttpStatusCode.NotFound, "Quiz not found");
        //    }

        //    if (quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
        //    {
        //        throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to delete this quiz");
        //    }

        //    await _quizRepository.Delete(quiz, true);

        //    return true;
        //}

        public async Task<QuizDto> UpdateQuiz(int id, Quiz_UpdateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update quiz");
            }

            var quizQuery = await _quizRepository.Get();
            quizQuery = quizQuery
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Lesson)
                    .ThenInclude(q => q.CourseSection)
                        .ThenInclude(q => q.Course)
                            .ThenInclude(q => q.Owner)
                .Where(q => q.Id == id);

            var quiz = await quizQuery.FirstOrDefaultAsync();

            if (quiz == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Quiz not found");
            }
            if (quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update this quiz");
            }

            if (requestDto.TimeLimit != null)
            {
                quiz.TimeLimit = requestDto.TimeLimit.Value;
            }
            await _quizRepository.Update(quiz, true);
            quiz = await quizQuery.FirstOrDefaultAsync();
            return _mapper.Map<QuizDto>(quiz);
        }
    }
}
