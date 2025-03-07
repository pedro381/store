using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Dtos
{
    public class OrderItemCreateDto
    {
        [Required]
        [DefaultValue(226)]
        public int ProductId { get; set; }

        [Required]
        [DefaultValue(2)]
        public int Quantity { get; set; }

        [Required]
        [DefaultValue(1.99)]
        public decimal UnitPrice { get; set; }
    }
}
