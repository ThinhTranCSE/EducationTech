using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Entities.Master
{
    [Index(nameof(TreeId))]
    public class Category : Abstract.Entity, INestedSetNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TreeId { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Category>(modelBuilder);
        }
    }
}
