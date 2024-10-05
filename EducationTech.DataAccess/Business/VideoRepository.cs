using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public VideoRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
