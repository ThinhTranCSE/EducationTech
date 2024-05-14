using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Shared.NestedSet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class CategorySeeder : Seeder
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategorySeeder(EducationTechContext context, ICategoryRepository categoryRepository) : base(context)
        {
            _categoryRepository = categoryRepository;
        }

        public override void Seed()
        {
            string jsonSeedData = @"
            [
              {
                ""Name"": ""Science & Engineering"",
                ""Children"": [
                  {
                    ""Name"": ""Basic Science"",
                    ""Children"": [
                      {""Name"": ""Mathematics""},
                      {""Name"": ""Physics""},
                      {""Name"": ""Chemistry""}
                    ]
                  },
                  {
                    ""Name"": ""Information Technology"",
                    ""Children"": [
                      {""Name"": ""Computer Science""},
                      {""Name"": ""Software Engineering""},
                      {""Name"": ""Data Science""}
                    ]
                  },
                  {
                    ""Name"": ""Construction Engineering"",
                    ""Children"": [
                      {""Name"": ""Civil Engineering""},
                      {""Name"": ""Mechanical Engineering""},
                      {""Name"": ""Electrical Engineering""}
                    ]
                  }
                ]
              },
              {
                ""Name"": ""Arts & Culture"",
                ""Children"": [
                  {
                    ""Name"": ""Painting & Graphics"",
                    ""Children"": [
                      {""Name"": ""Fine Arts""},
                      {""Name"": ""Graphic Design""},
                      {""Name"": ""Digital Art""}
                    ]
                  },
                  {
                    ""Name"": ""Music & Sound"",
                    ""Children"": [
                      {""Name"": ""Music Theory""},
                      {""Name"": ""Instrumental Music""},
                      {""Name"": ""Vocal Music""}
                    ]
                  },
                  {
                    ""Name"": ""Literature & Culture"",
                    ""Children"": [
                      {""Name"": ""English Literature""},
                      {""Name"": ""World Literature""},
                      {""Name"": ""Creative Writing""}
                    ]
                  }
                ]
              },
              {
                ""Name"": ""Business & Finance"",
                ""Children"": [
                  {
                    ""Name"": ""Business Management"",
                    ""Children"": [
                      {""Name"": ""Leadership""},
                      {""Name"": ""Strategic Management""},
                      {""Name"": ""Organizational Behavior""}
                    ]
                  },
                  {
                    ""Name"": ""Personal Finance"",
                    ""Children"": [
                      {""Name"": ""Budgeting""},
                      {""Name"": ""Investing""},
                      {""Name"": ""Retirement Planning""}
                    ]
                  },
                  {
                    ""Name"": ""Marketing & PR"",
                    ""Children"": [
                      {""Name"": ""Market Research""},
                      {""Name"": ""Brand Management""},
                      {""Name"": ""Digital Marketing""}
                    ]
                  }
                ]
              }
            ]";
            List<Node> nodes = JsonConvert.DeserializeObject<List<Node>>(jsonSeedData)!;
            foreach (var node in nodes)
            {
                InsertCategory(node, null);
            }

        }

        private void InsertCategory(Node node, Category? parent)
        {
            if (_categoryRepository.EntityNode.Any(c => c.Name == node.Name))
            {
                return;
            }
            var category = _categoryRepository.AddNode(parent, new Category
            {
                Name = node.Name,
                ParentId = parent?.Id
            });
            foreach(var childNode in node.Children)
            {
                InsertCategory(childNode, category);
            }
        }
    }

    internal class Node
    {
        public string Name { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();
    }
}
