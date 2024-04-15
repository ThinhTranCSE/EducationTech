using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public override DbSet<Video> Model => _context.Videos;
        public VideoRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
