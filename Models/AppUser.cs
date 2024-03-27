using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NehuenOrganico.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(100)]
        [MaxLength(256)]
        [Required]
        public string? Name { get; set; }
        [StringLength(100)]
        [MaxLength(256)]
        [Required]
        public string Address { get; set; }
        [Required(ErrorMessage = "Requiere el número")]
        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "El número de teléfono debe tener entre 7 y 15 dígitos.")]
        public string PhoneNumber { get; set; }

    }
}
