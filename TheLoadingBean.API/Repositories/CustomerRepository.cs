using MongoDB.Driver;
using TheLoadingBean.Shared.Models;
using TheLoadingBean.Shared.Interfaces;

namespace TheLoadingBean.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerRepository(IMongoDatabase database)
        {
            _customers = database.GetCollection<Customer>("customers");
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customers.Find(_ => true).ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(string id)
        {
            return await _customers.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _customers.Find(c => c.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(string id, Customer customer)
        {
            var result = await _customers.ReplaceOneAsync(c => c.Id == id, customer);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var result = await _customers.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
} 