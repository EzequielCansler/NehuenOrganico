using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NehuenOrganico.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Identity para auto incrementar
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public required string Name { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripcion no debe exeder los 255 caracteres")]
        public string? Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public float Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
    }
}

