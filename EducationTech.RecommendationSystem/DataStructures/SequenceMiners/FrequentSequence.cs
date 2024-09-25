namespace EducationTech.RecommendationSystem.DataStructures.SequenceMiners;

public class FrequentSequence<T>
{
    public List<List<T>> Sequence { get; set; }
    public int Support { get; set; }

    public FrequentSequence(List<List<T>> sequence, int support)
    {
        Sequence = sequence;
        Support = support;
    }
}
