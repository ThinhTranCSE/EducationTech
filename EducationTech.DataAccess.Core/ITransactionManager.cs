using Microsoft.EntityFrameworkCore.Storage;

namespace EducationTech.DataAccess.Core
{
    public interface ITransactionManager
    {
        IDbContextTransaction BeginTransaction();


        void CommitTransaction();
        void RollbackTransaction();
        void SaveChanges();

    }
}
