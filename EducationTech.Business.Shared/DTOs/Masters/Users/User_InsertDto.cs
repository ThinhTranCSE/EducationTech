﻿using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Masters.Users
{
    public class User_InsertDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

    }
}
