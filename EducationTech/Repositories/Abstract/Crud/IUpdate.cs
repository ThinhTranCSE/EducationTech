using EducationTech.Models.Abstract;

namespace EducationTech.Repositories.Abstract.Crud
{
    public interface IUpdate<T, TInsertDto> where T : IModel where TInsertDto : class
    {
        Task<T?> Update(TInsertDto dto);
    }
}
