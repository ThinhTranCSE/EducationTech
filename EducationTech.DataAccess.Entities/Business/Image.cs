using EducationTech.DataAccess.Entities.Abstract;
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
    public class Image : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid FileId { get; set; }
        public virtual UploadedFile File { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Image>(modelBuilder);
        }
    }
}
