using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class InstructorApprovedRepository : Repository<InstructorApproved>, IInstructorApprovedRepository
    {
        public override DbSet<InstructorApproved> Model => _context.InstructorApproveds;
        public InstructorApprovedRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
