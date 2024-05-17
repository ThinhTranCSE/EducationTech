using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IQuizRepository quizRepository, ITransactionManager transactionManager, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _quizRepository = quizRepository;
            _transactionManager = transactionManager;
            _mapper = mapper;
        }


        public async Task<QuestionDto> CreateQuestion(Question_CreateRequestDto requestDto, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new Exception("User not found");
            }

            var quizQuery = await _quizRepository.Get();
            quizQuery = quizQuery
                .Include(q => q.Lesson)
                    .ThenInclude(q => q.CourseSection)
                        .ThenInclude(q => q.Course)
                            .ThenInclude(q => q.Owner)
                .Where(x => x.Id == requestDto.QuizId);

            var quiz = await quizQuery.FirstOrDefaultAsync();

            if(quiz == null)
            {
                throw new Exception("Quiz not found");
            }

            if(quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new Exception("You are not allowed to create question for this quiz");
            }

            var createdQuestion = _mapper.Map<Question>(requestDto);

            await _questionRepository.Insert(createdQuestion, true);

            var question = await (await _questionRepository.Get())
                .Include(q => q.Answers)
                .Where(x => x.Id == createdQuestion.Id)
                .FirstOrDefaultAsync();

            return _mapper.Map<QuestionDto>(question);
        }

        public async Task<QuestionDto> UpdateQuestion(int id, Question_UpdateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new Exception("User not found");
            }

            var questionQuery = await _questionRepository.Get();    
            questionQuery = questionQuery
                .Include(q => q.Quiz)
                    .ThenInclude(q => q.Lesson)
                        .ThenInclude(q => q.CourseSection)
                            .ThenInclude(q => q.Course)
                                .ThenInclude(q => q.Owner)
                .Where(x => x.Id == id);

            var question = await questionQuery.FirstOrDefaultAsync();

            if (question == null)
            {
                throw new Exception("Question not found");
            }

            if (question.Quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new Exception("You are not allowed to update this question");
            }

            if(requestDto.Content != null)
            {
                question.Content = requestDto.Content;
            }

            await _questionRepository.Update(question, true);

            return _mapper.Map<QuestionDto>(question);
        }
    }
}
