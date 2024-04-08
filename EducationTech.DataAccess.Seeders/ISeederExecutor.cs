using Microsoft.Extensions.DependencyInjection;

namespace EducationTech.DataAccess.Seeders
{
    public interface ISeederExecutor
    {
        void Execute(CancellationTokenSource tokenSource, params string[] args);
        IDictionary<string, ISeeder> RegisterSeeders(IServiceScope scope);
    }
}
