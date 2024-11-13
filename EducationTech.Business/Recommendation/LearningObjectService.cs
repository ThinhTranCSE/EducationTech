using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;

namespace EducationTech.Business.Recommendation;

public class LearningObjectService : ILearningObjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LearningObjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<LearningObjectDto> CreateLearningObject(LearningObject_CreateRequest request)
    {
        var learningObject = _mapper.Map<LearningObject>(request);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.LearningObjects.Add(learningObject);
            _unitOfWork.SaveChanges();

            if (request.Type == LOType.Explanatory)
            {
                if (request.VideoId == null)
                {
                    throw new Exception("Video id is required for explanatory learning object");
                }

                var video = _unitOfWork.Videos.GetAll().FirstOrDefault(x => x.Id == request.VideoId);

                if (video == null)
                {
                    throw new Exception("Video not found");
                }

                video.LearningObjectId = learningObject.Id;

                _unitOfWork.SaveChanges();
            }

            transaction.Commit();

            return _mapper.Map<LearningObjectDto>(learningObject);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public async Task<LearningObjectDto> UpdateLearningObject(LearningObject_UpdateRequest request, int id)
    {
        var learningObject = _unitOfWork.LearningObjects.GetAll().FirstOrDefault(x => x.Id == id);

        if (learningObject == null)
        {
            throw new Exception("Learning object not found");
        }

        _mapper.Map(request, learningObject);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.SaveChanges();

            transaction.Commit();

            return _mapper.Map<LearningObjectDto>(learningObject);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public Task<bool> DeleteLearningObject(int id)
    {
        throw new NotImplementedException();
    }

}
