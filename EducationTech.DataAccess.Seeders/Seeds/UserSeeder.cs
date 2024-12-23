using Bogus;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.Shared.Enums;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class UserSeeder : Seeder
    {
        private readonly IAuthService _authService;
        public UserSeeder(EducationTechContext context, IAuthService authService) : base(context)
        {
            _authService = authService;
        }

        public override void Seed()
        {
            var dataGenerator = new Faker<RegisterDto>();

            var adminRoleId = _context.Roles.FirstOrDefault(x => x.Name == nameof(RoleType.Admin))!.Id;
            var instructorRoleId = _context.Roles.FirstOrDefault(x => x.Name == nameof(RoleType.Instructor))!.Id;
            var learnerRoleId = _context.Roles.FirstOrDefault(x => x.Name == nameof(RoleType.Learner))!.Id;


            var specialityIds = _context.Specialities.Select(x => x.Id).ToList();

            var adminGenerator = dataGenerator
                .RuleFor(x => x.Password, "12345678")
                .RuleFor(x => x.Email, f => f.Person.Email);


            var instructorGenerator = dataGenerator
                .RuleFor(x => x.Password, "12345678")
                .RuleFor(x => x.Email, f => f.Person.Email);

            var learnerGenerator = dataGenerator
                .RuleFor(x => x.Password, "12345678")
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.SpecialityId, f => f.PickRandom(specialityIds));



            var users = new List<RegisterDto>();

            var admin = adminGenerator.Generate();
            admin.Username = "admin";
            admin.RoleIds = new List<int> { adminRoleId };
            users.Add(admin);

            var instructor = instructorGenerator.Generate();
            instructor.Username = "instructor";
            instructor.RoleIds = new List<int> { instructorRoleId };
            users.Add(instructor);

            var instructors = instructorGenerator.Generate(5);
            int instructorIndex = 1;
            instructors.ForEach(i =>
            {
                i.Username = $"instructor{instructorIndex}";
                i.RoleIds = new List<int> { instructorRoleId };
                instructorIndex++;
            });
            users.AddRange(instructors);

            var learners = learnerGenerator.Generate(10);
            int learnerIndex = _context.Learners.Count() + 1;
            learners.ForEach(l =>
            {
                l.Username = $"learner{learnerIndex}";
                l.RoleIds = new List<int> { learnerRoleId };
                learnerIndex++;
            });
            users.AddRange(learners);


            users.ForEach(u => _authService.Register(u).GetAwaiter().GetResult());
        }
    }
}
