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
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public float Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}

