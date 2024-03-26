using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public override DbSet<Video> Model => _context.Videos;
        public VideoRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
