using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TheLoadingBean.Shared.Models;

namespace TheLoadingBeanAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoClient Client { get; }

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            Client = new MongoClient(settings.Value.ConnectionString);
            _database = Client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
    }
}
