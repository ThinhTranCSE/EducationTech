using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.DataAccess.Business;

public class UploadedFileRepository : Repository<UploadedFile>, IUploadedFileRepository
{
    public UploadedFileRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
