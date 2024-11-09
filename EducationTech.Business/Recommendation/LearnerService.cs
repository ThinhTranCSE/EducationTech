using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Learners;
using EducationTech.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class LearnerService : ILearnerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LearnerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<LearnerDto> UpdateLearner(UpdateLearnerRequest request)
    {
        var learner = _unitOfWork.Learners.GetAll()
            .Include(x => x.Speciality)
            .ThenInclude(x => x.Branch)
            .FirstOrDefault(x => x.Id == request.LearnerId);

        if (learner == null)
        {
            throw new Exception("Learner not found");
        }

        var speciality = _unitOfWork.Specialities.GetAll().FirstOrDefault(x => x.Id == request.SpecialityId);

        if (speciality == null)
        {
            throw new Exception("Speciality not found");
        }

        learner.SpecialityId = request.SpecialityId;

        _unitOfWork.SaveChanges();

        return _mapper.Map<LearnerDto>(learner);
    }
}
