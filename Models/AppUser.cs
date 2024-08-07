﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NehuenOrganico.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(100)]
        [Required]
        public string? Name { get; set; }
        [StringLength(100)]
        [Required]
        public string? Address { get; set; }
        

    }
}
