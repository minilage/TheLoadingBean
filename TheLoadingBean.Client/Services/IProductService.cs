using TheLoadingBean.Shared.DTOs;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto> GetProductByIdAsync(string id);
    Task<List<ProductResponseDto>> SearchProductsAsync(string searchTerm);
    Task<ProductResponseDto> CreateProductAsync(CreateProductDto product);
    Task<ProductResponseDto> UpdateProductAsync(string id, UpdateProductDto product);
    Task DeleteProductAsync(string id);
    Task<List<ProductResponseDto>> GetDiscontinuedProductsAsync();
    Task<List<ProductResponseDto>> GetAvailableProductsAsync();
}
