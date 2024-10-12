using AutoMapper;
using Newtonsoft.Json;

namespace EducationTech.Business.Shared.DTOs.Abstracts
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class AbstractDto<TEntity, TDto> : IDto
        where TDto : class
        where TEntity : class
    {
        public virtual void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TEntity, TDto>().ReverseMap();
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class AbstractDto : IDto
    {
        public virtual void Configure(IMapperConfigurationExpression cfg) { }
    }
}
