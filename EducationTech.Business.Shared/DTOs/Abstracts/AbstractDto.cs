using AutoMapper;
using EducationTech.Storage;
using Newtonsoft.Json;

namespace EducationTech.Business.Shared.DTOs.Abstracts
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class AbstractDto<TEntity, TDto> : IDto
        where TDto : class
        where TEntity : class
    {
        protected static GlobalReference GlobalUsings = GlobalReference.Instance;

        public virtual void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TEntity, TDto>().ReverseMap();
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class AbstractDto : IDto
    {
        //protected static GlobalUsings GlobalUsings = new GlobalUsings();
        public virtual void Configure(IMapperConfigurationExpression cfg) { }
    }
}
