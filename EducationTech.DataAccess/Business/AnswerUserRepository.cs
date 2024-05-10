using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Business
{
    public class AnswerUserRepository : Repository<AnswerUser>, IAnswerUserRepository
    {
        public override DbSet<AnswerUser> Model => _context.AnswerUsers;
        public AnswerUserRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
