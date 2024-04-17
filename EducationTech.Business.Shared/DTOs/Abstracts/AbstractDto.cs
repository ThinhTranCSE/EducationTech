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
    public abstract class AbstractDto : Profile
    {
        public abstract void Configure();
    }
}
