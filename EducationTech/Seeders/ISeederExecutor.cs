namespace EducationTech.Seeders
{
    public interface ISeederExecutor
    {
        void Execute(CancellationTokenSource tokenSource, params string[] args);
    }
}
