using EducationTech.RecommendationSystem.DataStructures;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.RecommendationSystem.Implementations.SequenceMiners
{
    public class PrefixSpanSequenceMiner<T> : ISequenceMiner<T>
    {
        // Hàm tính độ hỗ trợ (support) cho một chuỗi
        public int GetSupport(List<List<T>> prefix, List<Sequence<T>> database)
        {
            int support = 0;

            foreach (var sequence in database)
            {
                if (IsSubsequence(prefix, sequence.Itemsets))
                {
                    support++;
                }
            }

            return support;
        }

        // Kiểm tra nếu một prefix là subsequence của một chuỗi
        public bool IsSubsequence(List<List<T>> prefix, List<List<T>> sequence)
        {
            int j = 0, i = 0;

            while (i < sequence.Count && j < prefix.Count)
            {
                // Kiểm tra xem itemset trong sequence có chứa tất cả phần tử của prefix[j] không
                if (sequence[i].ContainsAll(prefix[j]))
                {
                    j++; // Di chuyển tới itemset tiếp theo của prefix
                }
                i++; // Di chuyển tới itemset tiếp theo của sequence
            }

            return j == prefix.Count; // Trả về true nếu tất cả itemsets của prefix được tìm thấy
        }

        // Tạo database chiếu (projected database) từ một prefix
        public List<List<T>> ProjectDatabase(List<List<T>> prefix, List<List<T>> sequence)
        {
            int j = 0;
            for (int i = 0; i < sequence.Count && j < prefix.Count; i++)
            {
                if (sequence[i].ContainsAll(prefix[j]))
                {
                    j++;
                }
            }

            // Trả về phần còn lại của chuỗi mà không cần tạo bản sao
            if (j == prefix.Count)
            {
                return sequence.Skip(j).ToList();
            }
            return new List<List<T>>();
        }


        // Hàm khai phá mẫu tuần tự sử dụng thuật toán PrefixSpan (sử dụng song song)
        public void MineSequences(List<List<T>> prefix, List<Sequence<T>> database, int minSupport, List<FrequentSequence<T>> result)
        {
            int support = GetSupport(prefix, database);

            if (support < minSupport)
                return;

            lock (result)
            {
                result.Add(new FrequentSequence<T>(prefix, support));
            }

            Dictionary<T, int> extensionSupport = new Dictionary<T, int>();

            // Khai phá các phần mở rộng song song để tối ưu
            Parallel.ForEach(database, sequence =>
            {
                List<List<T>> projectedDatabase = ProjectDatabase(prefix, sequence.Itemsets);
                foreach (var itemset in projectedDatabase)
                {
                    foreach (var item in itemset)
                    {
                        lock (extensionSupport)
                        {
                            if (!extensionSupport.ContainsKey(item))
                            {
                                extensionSupport[item] = 0;
                            }
                            extensionSupport[item]++;
                        }
                    }
                }
            });

            // Đệ quy khai phá các phần mở rộng có độ hỗ trợ đủ lớn
            Parallel.ForEach(extensionSupport, entry =>
            {
                if (entry.Value >= minSupport)
                {
                    List<List<T>> newPrefix = new List<List<T>>(prefix);
                    newPrefix.Add(new List<T> { entry.Key });
                    MineSequences(newPrefix, database, minSupport, result);
                }
            });
        }


    }

    public static class ExtensionMethods
    {
        // Hàm kiểm tra nếu một itemset chứa tất cả các phần tử của itemset khác
        public static bool ContainsAll<T>(this List<T> itemset, List<T> other)
        {
            return !other.Except(itemset).Any();
        }
    }
}
