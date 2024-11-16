
using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;

class AnswerService : IAnswerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AnswerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AnswerDto> CreateAnswer(Answer_CreateRequestDto request)
    {
        var answer = _mapper.Map<Answer>(request);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.Answers.Add(answer);
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

    public async Task<bool> DeleteAnswer(int id)
    {
        var answer = _unitOfWork.Answers.GetAll().FirstOrDefault(x => x.Id == id);

        if (answer == null)
        {
            throw new Exception("Answer not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.Answers.Remove(answer);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<AnswerDto> UpdateAnswer(int id, Answer_UpdateRequestDto requestDto)
    {
        var answer = _unitOfWork.Answers.GetAll().FirstOrDefault(x => x.Id == id);

        using var transaction = _unitOfWork.BeginTransaction();
        if (answer == null)
        {
            throw new Exception("Answer not found");
        }
        try
        {
            _mapper.Map(requestDto, answer);
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
