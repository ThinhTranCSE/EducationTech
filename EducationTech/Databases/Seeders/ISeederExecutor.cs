namespace EducationTech.Databases.Seeders
{
    public interface ISeederExecutor
    {
        void Execute(CancellationTokenSource tokenSource, params string[] args);
        IDictionary<string, ISeeder> RegisterSeeders(IServiceScope scope);
    }
}
