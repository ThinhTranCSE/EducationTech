using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class UploadedFileRepository : Repository<UploadedFile>, IUploadedFileRepository
    {
        public override DbSet<UploadedFile> Model => _context.UploadedFiles;
        public UploadedFileRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
