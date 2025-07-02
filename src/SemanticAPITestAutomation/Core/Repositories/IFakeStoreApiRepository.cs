namespace SemanticAPITestAutomation.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SemanticAPITestAutomation.Core.Models;

    public interface IFakeStoreApiRepository
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task DeleteProductAsync(int id);

        Task<IReadOnlyList<Cart>> GetAllCartsAsync();
        Task<Cart?> GetCartByIdAsync(int id);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<Cart> UpdateCartAsync(int id, Cart cart);
        Task DeleteCartAsync(int id);

        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
