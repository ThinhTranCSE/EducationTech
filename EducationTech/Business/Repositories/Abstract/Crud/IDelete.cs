using EducationTech.Business.Models.Abstract;

namespace EducationTech.Business.Repositories.Abstract.Crud
{
    public interface IDelete<T, TDeleteDto> where T : IModel where TDeleteDto : class
    {
        Task<T?> Delete(TDeleteDto dto);
    }
}
