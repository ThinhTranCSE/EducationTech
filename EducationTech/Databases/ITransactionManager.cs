using Microsoft.EntityFrameworkCore.Storage;

namespace EducationTech.Databases
{
    public interface ITransactionManager
    {
        IDbContextTransaction BeginTransaction();
        

        void CommitTransaction();
        void RollbackTransaction();
        void SaveChanges();

    }
}
 