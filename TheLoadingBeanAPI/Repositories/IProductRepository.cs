using TheLoadingBean.Shared.Models;

namespace TheLoadingBeanAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(string id, Product product);
        Task<bool> DeleteProductAsync(string id);
        Task<List<Product>> GetDiscontinuedProductsAsync();
        Task<List<Product>> GetAvailableProductsAsync();
    }
}
