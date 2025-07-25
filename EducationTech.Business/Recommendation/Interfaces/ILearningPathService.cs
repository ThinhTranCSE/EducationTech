﻿using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface ILearningPathService : IService
{
    //Task<OldLearningPathDto> RecommendLearningPath(int learnerId, int startUnitId, int targetUnitId);
    Task<LearningPathDto> RecomendLearningPathSemester(int learnerId, int specialityId);
    Task<bool> SaveLearningPath(LearningPath_SaveRequest request);
    Task<LearningPathDto?> LoadLearningPath();
    Task<LearningPath_ProgressDto?> LoadLearningPathProgress();

    Task<bool> AdjustLogToChangeSimilarity(int learnerId, string courseTitle, double targetSimilarity);
    Task<bool> MakeLearnerUseLearningPath(int learnerId);
};
