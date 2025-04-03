using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBean.Shared.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("CustomerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("Items")]
        public List<OrderItemDto> Items { get; set; } = new();

        [BsonElement("OrderDate")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [BsonElement("Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [BsonElement("TotalAmount")]
        public decimal TotalAmount { get; set; }
    }
}
