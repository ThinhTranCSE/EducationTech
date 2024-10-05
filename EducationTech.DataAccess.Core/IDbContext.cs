using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Core;

public interface IDbContext : IDisposable
{
    DbContext Instance { get; }
}
