using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IQuizService : IService
    {
        Task<QuizDto> CreateQuiz(Quiz_CreateRequestDto requestDto, User? currentUser);
        Task<QuizDto> UpdateQuiz(int id, Quiz_UpdateRequestDto requestDto, User? currentUser);

        //Task<bool> DeleteQuiz(int id, User? currentUser);

    }
}
