﻿using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Questions
{
    public class Question_CreateLessonResponseDto : AbstractDto<Question, Question_CreateLessonResponseDto>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public ICollection<Answer_CreateLessonResponseDto> Answers { get; set; } = new List<Answer_CreateLessonResponseDto>();
    }
}
