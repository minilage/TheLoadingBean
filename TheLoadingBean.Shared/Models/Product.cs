using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TheLoadingBean.Shared.Models

{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("ProductNumber")]
        public string ProductNumber { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("IsAvailable")]
        public bool IsAvailable { get; set; } = true;

        public bool IsDiscontinued { get; set; } = false;
    }
}
