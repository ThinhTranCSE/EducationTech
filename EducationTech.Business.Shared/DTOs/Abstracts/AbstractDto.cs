using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
