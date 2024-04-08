using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class ComunityRepository : Repository<Comunity>, IComunityRepository
    {
        public override DbSet<Comunity> Model => _context.Comunities;
        public ComunityRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
