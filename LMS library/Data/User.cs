﻿using System.ComponentModel.DataAnnotations;

namespace LMS_library.Data
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string email { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; } = new byte[32];
        public byte[] passwordSalt { get; set; } = new byte[32];


    }
}
