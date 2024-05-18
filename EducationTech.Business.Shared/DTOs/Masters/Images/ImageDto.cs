using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Images
{
    public class ImageDto : AbstractDto<Image, ImageDto>
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid FileId { get; set; }
    }
}
