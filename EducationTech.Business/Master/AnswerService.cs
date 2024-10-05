using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Master
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<AnswerDto> CreateAnswer(Answer_CreateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create answer");
            }

            var questionQuery = _unitOfWork.Questions.GetAll();
            questionQuery = questionQuery
                .Include(q => q.Quiz)
                    .ThenInclude(qz => qz.Lesson)
                        .ThenInclude(l => l.CourseSection)
                            .ThenInclude(cs => cs.Course)
                                .ThenInclude(c => c.Owner)
                .Where(q => q.Id == requestDto.QuestionId);

            var question = await questionQuery.FirstOrDefaultAsync();

            if (question == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Question not found");
            }

            if (question.Quiz.Lesson.CourseSection.Course.Owner.Id != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create answer for this question");
            }

            var answer = _mapper.Map<Answer>(requestDto);

            _unitOfWork.Answers.Add(answer);

            _unitOfWork.SaveChanges();

            return _mapper.Map<AnswerDto>(answer);
        }

        public async Task<AnswerDto> UpdateAnswer(int id, Answer_UpdateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update answer");
            }
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var answerQuery = _unitOfWork.Answers.GetAll();
                    answerQuery = answerQuery
                        .Include(a => a.Question)
                            .ThenInclude(q => q.Quiz)
                                .ThenInclude(qz => qz.Lesson)
                                    .ThenInclude(l => l.CourseSection)
                                        .ThenInclude(cs => cs.Course)
                                            .ThenInclude(c => c.Owner)
                        .Where(a => a.Id == id);

                    var answer = await answerQuery.FirstOrDefaultAsync();

                    if (answer == null)
                    {
                        throw new HttpException(HttpStatusCode.NotFound, "Answer not found");
                    }

                    if (answer.Question.Quiz.Lesson.CourseSection.Course.Owner.Id != currentUser.Id)
                    {
                        throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update answer for this question");
                    }

                    if (requestDto.Content != null)
                    {
                        answer.Content = requestDto.Content;
                    }

                    if (requestDto.IsCorrect != null)
                    {
                        answer.IsCorrect = requestDto.IsCorrect.Value;
                    }

                    _unitOfWork.SaveChanges();
                    transaction.Commit();

                    return _mapper.Map<AnswerDto>(answer);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
