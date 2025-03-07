using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class OrderItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }
    }
}
