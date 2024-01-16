using EducationTech.Models.Abstract;

namespace EducationTech.Repositories.Abstract.Crud
{
    public interface IInsert<T, TInsertDto> 
        where T : IModel
        where TInsertDto : class
    {
        Task<T?> Insert(TInsertDto insertDto);
        Task<ICollection<T>> Insert(IEnumerable<TInsertDto> insertDtos);
    }
}
