using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Quizzes;

public class Quiz_CreateRequest : AbstractDto<Quiz, Quiz_CreateRequest>
{
    public int LearningObjectId { get; set; }
    public int TimeLimit { get; set; }
}
