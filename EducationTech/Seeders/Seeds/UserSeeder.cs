using Bogus;
using EducationTech.Business.DTOs.Business.Auth;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Databases;
using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Master.Interfaces;
using EducationTech.Utilities.Interfaces;

namespace EducationTech.Seeders.Seeds
{
    public class UserSeeder : Seeder
    {
        private readonly IAuthService _authService;
        public UserSeeder(MainDatabaseContext context, IAuthService authService) : base(context)
        {
            _authService = authService;
        }

        public override void Seed()
        {
            var dataGenerator = new Faker<RegisterDto>();
            dataGenerator
                .RuleFor(x => x.Username, f => f.Person.UserName)
                .RuleFor(x => x.Password, "12345678")
                .RuleFor(x => x.DateOfBirth, f => f.Date.Past(20))
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.PhoneNumber, f => f.Person.Phone);

            var users = dataGenerator.Generate(5);
            var createdUsers = users.Select(u => _authService.Register(u));
            Task.WaitAll(createdUsers.ToArray());
        }
    }
}
