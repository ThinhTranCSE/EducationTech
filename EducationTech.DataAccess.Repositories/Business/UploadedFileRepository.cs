using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class UploadedFileRepository : Repository<UploadedFile>, IUploadedFileRepository
    {
        public override DbSet<UploadedFile> Model => _context.UploadedFiles;
        public UploadedFileRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
