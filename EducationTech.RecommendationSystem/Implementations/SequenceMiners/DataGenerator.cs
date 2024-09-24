using EducationTech.RecommendationSystem.DataStructures;

namespace EducationTech.RecommendationSystem.Implementations.SequenceMiners
{
    public class DataGenerator
    {
        private static Random random = new Random();

        // Tạo bộ dữ liệu gồm 1000 sequence và 100 phần tử
        public static List<Sequence<string>> GenerateData(int numSequences, int numItems, int minSupport)
        {
            // Tạo danh sách các item (ví dụ: "Item1", "Item2", ..., "Item100")
            List<string> items = Enumerable.Range(1, numItems).Select(i => $"Item{i}").ToList();

            // Tạo bộ dữ liệu
            List<Sequence<string>> database = new List<Sequence<string>>();

            // Các chuỗi mẫu (được lặp lại nhiều lần để đảm bảo support >= minSupport)
            List<List<List<string>>> frequentPatterns = new List<List<List<string>>>
            {
                new List<List<string>> { new List<string> { "Item1" }, new List<string> { "Item2" } },
                new List<List<string>> { new List<string> { "Item3", "Item4" }, new List<string> { "Item5" } },
                new List<List<string>> { new List<string> { "Item6" }, new List<string> { "Item7", "Item8" } },
                new List<List<string>> { new List<string> { "Item9" }, new List<string> { "Item10" } },
                new List<List<string>> { new List<string> { "Item11", "Item12" }, new List<string> { "Item13" } }
            };

            // Lặp lại mỗi mẫu ít nhất minSupport lần để đảm bảo điều kiện
            foreach (var pattern in frequentPatterns)
            {
                for (int i = 0; i < minSupport; i++)
                {
                    database.Add(new Sequence<string>(pattern));
                }
            }

            // Tạo các sequence ngẫu nhiên còn lại
            for (int i = database.Count; i < numSequences; i++)
            {
                database.Add(GenerateRandomSequence(items, random.Next(1, 5)));
            }

            return database;
        }

        // Hàm tạo một sequence ngẫu nhiên
        private static Sequence<string> GenerateRandomSequence(List<string> items, int numItemsets)
        {
            List<List<string>> sequence = new List<List<string>>();

            for (int i = 0; i < numItemsets; i++)
            {
                // Mỗi itemset chứa từ 1 đến 3 phần tử ngẫu nhiên
                List<string> itemset = items.OrderBy(x => random.Next()).Take(random.Next(1, 4)).ToList();
                sequence.Add(itemset);
            }

            return new Sequence<string>(sequence);
        }
    }
}
