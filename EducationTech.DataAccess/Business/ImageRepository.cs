using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Business
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public override DbSet<Image> Model => _context.Images;
        public ImageRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
