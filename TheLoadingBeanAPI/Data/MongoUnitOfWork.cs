using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TheLoadingBeanAPI.Repositories;

namespace TheLoadingBeanAPI.Data
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;
        private IClientSessionHandle? _session;
        private bool _disposed;

        private IProductRepository? _products;
        private ICustomerRepository? _customers;
        private IOrderRepository? _orders;

        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);

        public MongoUnitOfWork(IOptions<MongoDbSettings> settings)
        {
            _context = new MongoDbContext(settings);
        }

        public async Task BeginTransactionAsync()
        {
            _session = await _context.Client.StartSessionAsync();
            _session.StartTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            if (_session == null)
                throw new InvalidOperationException("No transaction is in progress.");

            await _session.CommitTransactionAsync();
            _session.Dispose();
            _session = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_session == null)
                throw new InvalidOperationException("No transaction is in progress.");

            await _session.AbortTransactionAsync();
            _session.Dispose();
            _session = null;
        }

        public Task<int> SaveChangesAsync()
        {
            // MongoDB automatically saves changes when operations are performed
            // This method is included for compatibility with other ORMs
            return Task.FromResult(1);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _session?.Dispose();
                    _products = null;
                    _customers = null;
                    _orders = null;
                }
                _disposed = true;
            }
        }
    }
} 