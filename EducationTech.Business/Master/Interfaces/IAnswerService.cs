using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Answers;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IAnswerService : IService
    {
        Task<AnswerDto> CreateAnswer(Answer_CreateRequestDto requestDto, User? currentUser);
        Task<AnswerDto> UpdateAnswer(int id, Answer_UpdateRequestDto requestDto, User? currentUser);
    }
}
