using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Entities
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}
