using EducationTech.DataAccess.Core;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTech.DataAccess.Seeders
{
    public class SeederExecutor : ISeederExecutor
    {
        private readonly EducationTechContext _context;
        private IDictionary<string, ISeeder> _seeders { get; set; }

        public SeederExecutor(EducationTechContext context)
        {
            _context = context;

        }

        public IDictionary<string, ISeeder> RegisterSeeders(IServiceScope scope)
        {
            IEnumerable<Type> seederTypes = typeof(ISeeder).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .Where(t => t.IsAssignableTo(typeof(ISeeder)));

            IDictionary<string, ISeeder> seeders = seederTypes.Select(t =>
            {
                if (t.IsAbstract || t.IsInterface)
                {
                    throw new Exception($"Seeder {t.Name} cannot be abstract or interface");
                }
                if (!t.IsAssignableTo(typeof(ISeeder)))
                {
                    throw new Exception($"Seeder {t.Name} must implement ISeeder");
                }
                return new KeyValuePair<string, ISeeder>(t.Name, (ISeeder)scope.ServiceProvider.GetRequiredService(t));
            })
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            _seeders = seeders;
            return seeders;


        }

        private void Seed()
        {
            foreach (ISeeder seeder in _seeders.Values)
            {
                seeder.Seed();
            }
        }

        private void Seed(string seederName)
        {
            if (!_seeders.ContainsKey(seederName))
            {
                throw new Exception($"Seeder {seederName} not found");
            }
            _seeders[seederName].Seed();
        }

        public void Execute(CancellationTokenSource tokenSource, params string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            else if (args.Length == 1)
            {
                if (args[0] != "seeder")
                {
                    return;
                }
                Seed();
                tokenSource.Cancel();
            }
            else
            {
                if (args[0] != "seeder")
                {
                    return;
                }
                foreach (string seederName in args.Skip(1))
                {
                    Seed(seederName);
                }
                tokenSource.Cancel();
            }
        }
    }
}
