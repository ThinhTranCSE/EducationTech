using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class ComunityRepository : Repository<Comunity>, IComunityRepository
    {
        public override DbSet<Comunity> Model => _context.Comunities;
        public ComunityRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
