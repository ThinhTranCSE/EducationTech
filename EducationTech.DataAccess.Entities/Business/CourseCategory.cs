using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Entities.Business
{
    public class CourseCategory : Abstract.Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Category Category { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<CourseCategory>(modelBuilder);
        }
    }
}
