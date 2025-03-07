using Store.Domain.Base;

namespace Store.Domain.Dtos
{
    public class OrderDto: ResponseBase
    {
        public int OrderId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = [];
    }
}
