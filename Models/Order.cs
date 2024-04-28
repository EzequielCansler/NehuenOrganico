using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NehuenOrganico.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "Requiere el nombre")]
        public string? Name { get; set; } = string.Empty;
        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "El número de teléfono debe tener entre 7 y 15 dígitos.")]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Shipping { get; set; }
        public string? Comments { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a cero")]
        public double Total { get; set; }
        //FK's
        public int? StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }
        //
        public string? Id { get; set; }
        [ForeignKey("Id")]
        public AppUser AppUser { get; set; }

    }
}
