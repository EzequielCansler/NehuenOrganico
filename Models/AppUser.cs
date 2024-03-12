using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NehuenOrganico.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(100)]
        [MaxLength(256)]
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }


        

    }
}
