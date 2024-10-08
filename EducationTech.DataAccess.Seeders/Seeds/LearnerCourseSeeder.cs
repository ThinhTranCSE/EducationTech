using Bogus;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class LearnerCourseSeeder : Seeder
    {
        public override int Piority => 2;

        public LearnerCourseSeeder(EducationTechContext context) : base(context)
        {

        }

        public override void Seed()
        {
            var users = _context.Users.ToList();
            var courses = _context.Courses.ToList();

            var learnerCourses = new List<LearnerCourse>();
            var leanerCourseGenrator = new Faker<LearnerCourse>()
                .RuleFor(x => x.LearnerId, f => users[f.Random.Int(0, users.Count - 1)].Id)
                .RuleFor(x => x.Rate, f => f.Random.Double(0, 5));

            foreach (var course in courses)
            {
                var learnerCoursesForCourse = leanerCourseGenrator.Generate(50);
                foreach (var learnerCourse in learnerCoursesForCourse)
                {
                    learnerCourse.CourseId = course.Id;
                    learnerCourses.Add(learnerCourse);
                }
            }
            _context.LearnerCourses.AddRange(learnerCourses);
            _context.SaveChanges();
        }
    }
}
