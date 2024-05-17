using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, ITransactionManager transactionManager, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _transactionManager = transactionManager;
            _mapper = mapper;
        }


        public async Task<AnswerDto> CreateAnswer(Answer_CreateRequestDto requestDto, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create answer");
            }

            var questionQuery = await _questionRepository.Get();
            questionQuery = questionQuery
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.Lesson)
                        .ThenInclude(l => l.CourseSection)
                            .ThenInclude(cs => cs.Course)
                                .ThenInclude(c => c.Owner)
                .Where(q => q.Id == requestDto.QuestionId);

            var question = await questionQuery.FirstOrDefaultAsync();

            if(question == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Question not found");
            }

            if(question.Quiz.Lesson.CourseSection.Course.Owner.Id != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create answer for this question");
            }

            var answer = _mapper.Map<Answer>(requestDto);
                            
            await _answerRepository.Insert(answer, true);

            return _mapper.Map<AnswerDto>(answer);
        }

        public async Task<AnswerDto> UpdateAnswer(int id, Answer_UpdateRequestDto requestDto, User? currentUser)
        {
            if(currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update answer");
            }

            var answerQuery = await _answerRepository.Get();
            answerQuery = answerQuery
                .Include(a => a.Question)
                    .ThenInclude(q => q.Quiz)
                        .ThenInclude(qz => qz.Lesson)
                            .ThenInclude(l => l.CourseSection)
                                .ThenInclude(cs => cs.Course)
                                    .ThenInclude(c => c.Owner)
                .Where(a => a.Id == id);

            var answer = await answerQuery.FirstOrDefaultAsync();

            if(answer == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Answer not found");
            }

            if(answer.Question.Quiz.Lesson.CourseSection.Course.Owner.Id != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update answer for this question");
            }

            if(requestDto.Content != null)
            {
                answer.Content = requestDto.Content;
            }

            if(requestDto.IsCorrect != null)
            {
                answer.IsCorrect = requestDto.IsCorrect.Value;
            }

            await _answerRepository.Update(answer, true);

            return _mapper.Map<AnswerDto>(answer);
        }
    }
}
