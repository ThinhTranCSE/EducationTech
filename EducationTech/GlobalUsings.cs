namespace EducationTech
{
    public class GlobalUsings
    {
        private readonly IWebHostEnvironment _env;

        public string ContentRootPath => _env.ContentRootPath;
        public string StaticFilesPath => Path.Combine(ContentRootPath, "Static");
        public GlobalUsings(IWebHostEnvironment env)
        {
            _env = env;
        }
    }
}
