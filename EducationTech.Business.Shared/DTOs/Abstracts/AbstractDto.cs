using AutoMapper;
using EducationTech.Storage;
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
        protected static GlobalUsings GlobalUsings = new GlobalUsings();

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
