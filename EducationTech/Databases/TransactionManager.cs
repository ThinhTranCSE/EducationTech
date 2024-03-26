using Microsoft.EntityFrameworkCore.Storage;

namespace EducationTech.Databases
{
    public class TransactionManager : ITransactionManager
    {
        private readonly EducationTechContext _context;

        public TransactionManager(EducationTechContext context)
        {
            _context = context;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.CurrentTransaction ?? _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }   

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
