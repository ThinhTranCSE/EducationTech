﻿using EducationTech.RecommendationSystem.DataStructures.SequenceMiners;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ISequenceMiner<T>
{
    public void MineSequences(List<List<T>> prefix, List<Sequence<T>> database, int minSupport, List<FrequentSequence<T>> result);
}
