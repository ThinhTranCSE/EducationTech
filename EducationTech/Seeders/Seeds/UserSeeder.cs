using Bogus;
using EducationTech.Databases;
using EducationTech.DTOs.Business.Auth;
using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Services.Business.Interfaces;
using EducationTech.Services.Master.Interfaces;
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
