using MediatR;
using Store.Domain.Dtos;

namespace Store.Application.Queries
{
    public class GetOrdersQuery : IRequest<(IEnumerable<OrderDto>, int)>
    {
        public GetOrdersQuery()
        {
        }

        public GetOrdersQuery(int pageNumber, int pageSize, string order)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.OrderBy = order;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = string.Empty;
    }
}
