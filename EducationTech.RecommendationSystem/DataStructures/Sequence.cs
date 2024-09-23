namespace EducationTech.RecommendationSystem.DataStructures;

public class Sequence<T>
{
    public List<List<T>> Itemsets { get; set; }

    public Sequence(List<List<T>> itemsets)
    {
        Itemsets = itemsets;
    }
}
