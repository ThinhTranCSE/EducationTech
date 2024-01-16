using EducationTech.Models.Abstract;

namespace EducationTech.Repositories.Abstract.Crud
{
    public interface IDelete<T, TDeleteDto> where T : IModel where TDeleteDto : class
    {
        Task<T?> Delete(TDeleteDto dto);
    }
}
