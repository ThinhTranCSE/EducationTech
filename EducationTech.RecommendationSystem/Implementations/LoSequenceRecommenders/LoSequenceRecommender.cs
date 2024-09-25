using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.DataStructures.SequenceMiners;
using EducationTech.RecommendationSystem.Implementations.LearnerCollaborativeFilters;
using EducationTech.RecommendationSystem.Implementations.LoRecommenders;
using EducationTech.RecommendationSystem.Implementations.SequenceMiners;
using EducationTech.RecommendationSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.RecommendationSystem.Implementations.LoSequenceRecommenders;

public class LoSequenceRecommender : ILoSequenceRecommender
{
    private const int MIN_SUPPORT = 3;
    private const int NUMBER_OF_SIMILAR_LEARNERS = 50;
    private const int NUMBER_OF_RECOMMENDATION_FOR_LOS = 5;
    private const int VISITED_TIME_THRESHOLD = 0;

    private readonly ILearnerCollaborativeFilter _learnerCollaborativeFilter;
    private readonly ILoRecommender _loRecommender;
    private readonly ISequenceMiner<int> _sequenceMiner;

    private readonly ILearningObjectRepository _learningObjectRepository;
    private readonly ILearnerRepository _learnerRepository;
    private readonly ILearnerLogRepository _learnerLogRepository;
    public LoSequenceRecommender(ILearningObjectRepository learningObjectRepository, ILearnerRepository learnerRepository, ILearnerLogRepository learnerLogRepository)
    {
        _learnerCollaborativeFilter = new CosineSimilarityLearnerFilter(NUMBER_OF_SIMILAR_LEARNERS);
        _loRecommender = new SimilarUserRatingLoRecommender(_learnerCollaborativeFilter);
        _sequenceMiner = new PrefixSpanSequenceMiner<int>();

        _learningObjectRepository = learningObjectRepository;
        _learnerRepository = learnerRepository;
        _learnerLogRepository = learnerLogRepository;
    }
    public async Task<List<FrequentSequence<int>>> RecommendTopNLearningObjectSequences(Learner learner, RecommendTopic searchedTopic)
    {
        //lấy tất cả learner (do trong thuật toán cần so sánh với tất cả learner)
        var learnerQuery = await _learnerRepository.Get();
        learnerQuery = learnerQuery.Include(x => x.LearningStyle).Include(x => x.LearnerLogs);

        var interestedLearners = await learnerQuery.ToListAsync();

        var similarLearnersLookUpTable = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, interestedLearners);
        var similarLearners = similarLearnersLookUpTable.Select(x => x.Key).ToList();
        var similarLearnerIdsHashedSet = similarLearners.Select(x => x.Id).ToHashSet();

        //chỉ quan tâm LOs của topic mà learner search
        var learningObjectQuery = await _learningObjectRepository.Get();
        learningObjectQuery = learningObjectQuery
            .Include(x => x.LearnerLogs)
            .ThenInclude(x => x.Learner)
            .Include(x => x.Topic)
            .Where(x => x.TopicId == searchedTopic.Id);

        var interestedLearningObjects = await learningObjectQuery.ToListAsync();

        var topNLos = await _loRecommender.RecommendTopNLearningObjects(learner, similarLearners, interestedLearningObjects, NUMBER_OF_RECOMMENDATION_FOR_LOS);
        var topNLoIdsHashedSet = topNLos.Select(x => x.Id).ToHashSet();

        var learnerLogQuery = await _learnerLogRepository.Get();
        learnerLogQuery = learnerLogQuery
            .Include(x => x.Learner)
            .Include(x => x.LearningObject)
            .Where(x => similarLearnerIdsHashedSet.Contains(x.LearnerId))
            .Where(x => x.LearningObject.TopicId == searchedTopic.Id)
            .Where(x => x.VisitedTime > VISITED_TIME_THRESHOLD);

        var learnerLogs = await learnerLogQuery.ToListAsync();

        //group learnerLogs theo learnerId để tìm sequences
        var learnerLogsGroupByLearnerIdDictionary = new Dictionary<int, SortedList<DateTime, LearnerLog>>();
        foreach (var learnerLog in learnerLogs)
        {
            if (!learnerLogsGroupByLearnerIdDictionary.ContainsKey(learnerLog.LearnerId))
            {
                learnerLogsGroupByLearnerIdDictionary.Add(learnerLog.LearnerId, new SortedList<DateTime, LearnerLog>());
            }

            learnerLogsGroupByLearnerIdDictionary[learnerLog.LearnerId].Add(learnerLog.VisitedAt, learnerLog);
        }

        //remove những learnerLogs trong SortedList xuất hiện trước một trong các topNLos
        foreach (var (learnerId, logs) in learnerLogsGroupByLearnerIdDictionary)
        {
            // Tạo danh sách các key cần remove
            var keysToRemove = new List<DateTime>();

            //đi từ thấp đến cao để remove do đã sorted
            foreach (var (visitedAt, log) in logs)
            {
                if (topNLoIdsHashedSet.Contains(log.LearningObjectId))
                {
                    break; // Dừng lại khi gặp log thuộc topNLos
                }
                keysToRemove.Add(visitedAt); // Đánh dấu key để xóa
            }

            if (logs.Count - keysToRemove.Count == 0)
            {
                learnerLogsGroupByLearnerIdDictionary.Remove(learnerId);
                continue;
            }
            // Xóa các key đã đánh dấu
            foreach (var key in keysToRemove)
            {
                logs.Remove(key);
            }
        }

        //tạo sequences
        var sequenceDatabase = new List<Sequence<int>>();
        foreach (var (_, logs) in learnerLogsGroupByLearnerIdDictionary)
        {
            var itemSet = logs.Values.Select(i => new List<int> { i.LearningObjectId }).ToList();
            sequenceDatabase.Add(new Sequence<int>(itemSet));
        }
        //[[1,2,3],[3,4,5,6]]
        //var databaseString = "[";
        //foreach (var sequence in sequenceDatabase)
        //{
        //    databaseString += "[";
        //    foreach (var itemset in sequence.Itemsets)
        //    {
        //        foreach (var item in itemset)
        //        {
        //            databaseString += item + ",";
        //        }
        //    }
        //    databaseString = databaseString.Remove(databaseString.Length - 1);
        //    databaseString += "],";
        //}
        //databaseString = databaseString.Remove(databaseString.Length - 1);


        //tìm frequent sequences
        var prefix = new List<List<int>>();
        var frequentSequences = new List<FrequentSequence<int>>();
        _sequenceMiner.MineSequences(prefix, sequenceDatabase, MIN_SUPPORT, frequentSequences);

        return frequentSequences;
    }
}
