using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Dtos
{
    public class OrderCreateDto
    {
        [Required]
        [DefaultValue("S001")]
        public string Number { get; set; } = string.Empty;
        [Required]
        [DefaultValue("Pedro Souza")]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        [DefaultValue("PIX")]
        public string PaymentMethod { get; set; } = string.Empty;
        [Required]
        public List<OrderItemCreateDto> OrderItems { get; set; } = [];
    }
}
