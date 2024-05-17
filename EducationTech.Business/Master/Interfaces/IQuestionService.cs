using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Questions;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IQuestionService : IService
    {
        Task<QuestionDto> CreateQuestion(Question_CreateRequestDto requestDto, User? currentUser);
        Task<QuestionDto> UpdateQuestion(int id, Question_UpdateRequestDto requestDto, User? currentUser);
    }
}
