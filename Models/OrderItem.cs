using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NehuenOrganico.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

    }
}
