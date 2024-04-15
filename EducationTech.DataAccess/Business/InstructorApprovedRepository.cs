using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class InstructorApprovedRepository : Repository<InstructorApproved>, IInstructorApprovedRepository
    {
        public override DbSet<InstructorApproved> Model => _context.InstructorApproveds;
        public InstructorApprovedRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
