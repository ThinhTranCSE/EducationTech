using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Core.Contexts.Interfaces;

public interface IDbContext : IDisposable
{
    DbContext Instance { get; }
}
