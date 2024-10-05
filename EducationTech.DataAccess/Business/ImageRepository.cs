using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
