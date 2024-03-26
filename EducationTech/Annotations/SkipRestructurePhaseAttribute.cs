namespace EducationTech.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipRestructurePhaseAttribute : Attribute
    {
        public SkipRestructurePhaseAttribute() { }
    }
}
