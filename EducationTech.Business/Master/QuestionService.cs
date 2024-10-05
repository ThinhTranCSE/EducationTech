using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Master
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<QuestionDto> CreateQuestion(Question_CreateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new Exception("User not found");
            }

            var quizQuery = _unitOfWork.Quizzes.GetAll();
            quizQuery = quizQuery
                .Include(q => q.Lesson)
                    .ThenInclude(q => q.CourseSection)
                        .ThenInclude(q => q.Course)
                            .ThenInclude(q => q.Owner)
                .Where(x => x.Id == requestDto.QuizId);

            var quiz = await quizQuery.FirstOrDefaultAsync();

            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }

            if (quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new Exception("You are not allowed to create question for this quiz");
            }

            var createdQuestion = _mapper.Map<Question>(requestDto);

            _unitOfWork.Questions.Add(createdQuestion);
            _unitOfWork.SaveChanges();

            var question = await _unitOfWork.Questions.GetAll()
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

            var questionQuery = _unitOfWork.Questions.GetAll();
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

            if (requestDto.Content != null)
            {
                question.Content = requestDto.Content;
            }

            _unitOfWork.SaveChanges();

            return _mapper.Map<QuestionDto>(question);
        }
    }
}
