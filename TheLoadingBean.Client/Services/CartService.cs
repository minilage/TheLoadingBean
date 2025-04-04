using System.Net.Http.Json;

public class CartService : ICartService
{
    private readonly HttpClient _http;

    public CartService(HttpClient http)
    {
        _http = http;
    }

    public async Task AddToCartAsync(string productId)
    {
        await _http.PostAsJsonAsync("api/cart", new { ProductId = productId });
    }
}
