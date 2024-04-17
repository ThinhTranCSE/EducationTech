using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Abstracts
{
    public interface IDto
    {
        void Configure(IMapperConfigurationExpression cfg);  
    }
}
