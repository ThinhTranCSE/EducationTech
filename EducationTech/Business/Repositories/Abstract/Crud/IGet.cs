using EducationTech.Business.Models.Abstract;

namespace EducationTech.Business.Repositories.Abstract.Crud
{
    public interface IGet<T, TGetDto>
        where T : IModel
        where TGetDto : class
    {
        Task<ICollection<T>> Get(TGetDto getDto);
    }
}
