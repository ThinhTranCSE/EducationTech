using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Quizzes
{
    public class Quiz_UpdateRequestDto : AbstractDto<Quiz, Quiz_UpdateRequestDto>
    {
        public int? TimeLimit { get; set; }

    }
}
