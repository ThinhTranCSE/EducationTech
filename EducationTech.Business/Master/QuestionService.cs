
using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;

class QuestionService : IQuestionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuestionDto> CreateQuestion(Question_CreateRequestDto request)
    {
        var question = _mapper.Map<Question>(request);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.Questions.Add(question);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return _mapper.Map<QuestionDto>(question);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<QuestionDto> UpdateQuestion(Question_UpdateRequestDto request, int id)
    {
        var question = _unitOfWork.Questions.GetAll().FirstOrDefault(x => x.Id == id);

        if (question == null)
        {
            throw new Exception("Question not found");
        }

        _mapper.Map(request, question);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return _mapper.Map<QuestionDto>(question);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
