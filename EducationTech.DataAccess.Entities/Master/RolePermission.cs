﻿using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    public class RolePermission : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }


        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<RolePermission>(modelBuilder);
        }
    }
}
