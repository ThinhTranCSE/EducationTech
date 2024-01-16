using Bogus;
using EducationTech.Databases;
using EducationTech.Enums;
using EducationTech.Models.Master;

namespace EducationTech.Seeders.Seeds
{
    public class UserSeeder : Seeder
    {
        public UserSeeder(MainDatabaseContext context) : base(context)
        {
        }

        public override void Seed()
        {
            Faker<User> dataGenerator = new Faker<User>();
            dataGenerator
                .RuleFor(x => x.Username, f => f.Person.UserName)
                .RuleFor(x => x.Password, f => f.Internet.Password())
                .RuleFor(x => x.DateOfBirth, f => f.Date.Past(20))
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
                .RuleFor(x => x.Role, f => f.PickRandom<Role>());


            User[] users = dataGenerator.Generate(10).ToArray();
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }
}
