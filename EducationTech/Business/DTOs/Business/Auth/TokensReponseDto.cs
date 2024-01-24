using EducationTech.Business.Models.Master;

namespace EducationTech.Business.DTOs.Business.Auth
{
    public class TokensReponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
