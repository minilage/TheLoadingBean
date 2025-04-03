using System.ComponentModel.DataAnnotations;
using TheLoadingBean.Shared.Models;

namespace TheLoadingBean.Shared.DTOs
{
    public class OrderResponseDto
    {
        public OrderResponseDto()
        {
            Id = string.Empty;
            CustomerId = string.Empty;
            Items = new List<OrderItemDto>();
        }

        public string Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class CreateOrderDto
    {
        public CreateOrderDto()
        {
            CustomerId = string.Empty;
            Items = new List<OrderItemDto>();
        }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        [MinLength(1)]
        public List<OrderItemDto> Items { get; set; }
    }

    public class UpdateOrderDto
    {
        public UpdateOrderDto()
        {
            Items = new List<OrderItemDto>();
        }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        [MinLength(1)]
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public OrderItemDto()
        {
            ProductId = string.Empty;
        }

        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public UpdateOrderStatusDto()
        {
            OrderId = string.Empty;
            Status = string.Empty;
        }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
