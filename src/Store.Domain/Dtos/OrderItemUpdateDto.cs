using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Dtos
{
    public class OrderItemUpdateDto
    {
        [Required]
        public int OrderItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "msgQuantity", ErrorMessageResourceType = typeof(Resource))]
        public int Quantity { get; set; }

    }
}
