namespace SemanticAPITestAutomation.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RestSharp;

    using SemanticAPITestAutomation.Core.Models;

    public class FakeStoreApiRepository : IFakeStoreApiRepository
    {
        private readonly RestClient _client;

        public FakeStoreApiRepository(RestClient client)
        {
            this._client = client;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            RestRequest request = new RestRequest("products");
            RestResponse<List<Product>> response = await this._client.ExecuteAsync<List<Product>>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to get products: {response.ErrorMessage}")
                : (IReadOnlyList<Product>)response.Data.AsReadOnly();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            RestRequest request = new RestRequest($"products/{id}");
            RestResponse<Product> response = await this._client.ExecuteAsync<Product>(request);

            return response.IsSuccessful ? response.Data : null;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            RestRequest request = new RestRequest("products", Method.Post)
                .AddJsonBody(product);

            RestResponse<Product> response = await this._client.ExecuteAsync<Product>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to create product: {response.ErrorMessage}")
                : response.Data;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            RestRequest request = new RestRequest($"products/{id}", Method.Put)
                .AddJsonBody(product);

            RestResponse<Product> response = await this._client.ExecuteAsync<Product>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to update product: {response.ErrorMessage}")
                : response.Data;
        }

        public async Task DeleteProductAsync(int id)
        {
            RestRequest request = new RestRequest($"products/{id}", Method.Delete);
            RestResponse response = await this._client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException($"Failed to delete product: {response.ErrorMessage}");
            }
        }

        public async Task<IReadOnlyList<Cart>> GetAllCartsAsync()
        {
            RestRequest request = new RestRequest("carts");
            RestResponse<List<Cart>> response = await this._client.ExecuteAsync<List<Cart>>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to get carts: {response.ErrorMessage}")
                : (IReadOnlyList<Cart>)response.Data.AsReadOnly();
        }

        public async Task<Cart?> GetCartByIdAsync(int id)
        {
            RestRequest request = new RestRequest($"carts/{id}");
            RestResponse<Cart> response = await this._client.ExecuteAsync<Cart>(request);

            return response.IsSuccessful ? response.Data : null;
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            RestRequest request = new RestRequest("carts", Method.Post)
                .AddJsonBody(cart);

            RestResponse<Cart> response = await this._client.ExecuteAsync<Cart>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to create cart: {response.ErrorMessage}")
                : response.Data;
        }

        public async Task<Cart> UpdateCartAsync(int id, Cart cart)
        {
            RestRequest request = new RestRequest($"carts/{id}", Method.Put)
                .AddJsonBody(cart);

            RestResponse<Cart> response = await this._client.ExecuteAsync<Cart>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to update cart: {response.ErrorMessage}")
                : response.Data;
        }

        public async Task DeleteCartAsync(int id)
        {
            RestRequest request = new RestRequest($"carts/{id}", Method.Delete);
            RestResponse response = await this._client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException($"Failed to delete cart: {response.ErrorMessage}");
            }
        }

        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            RestRequest request = new RestRequest("users");
            RestResponse<List<User>> response = await this._client.ExecuteAsync<List<User>>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to get users: {response.ErrorMessage}")
                : (IReadOnlyList<User>)response.Data.AsReadOnly();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            RestRequest request = new RestRequest($"users/{id}");
            RestResponse<User> response = await this._client.ExecuteAsync<User>(request);

            return response.IsSuccessful ? response.Data : null;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            RestRequest request = new RestRequest("users", Method.Post)
                .AddJsonBody(user);

            RestResponse<User> response = await this._client.ExecuteAsync<User>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to create user: {response.ErrorMessage}")
                : response.Data;
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            RestRequest request = new RestRequest($"users/{id}", Method.Put)
                .AddJsonBody(user);

            RestResponse<User> response = await this._client.ExecuteAsync<User>(request);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to update user: {response.ErrorMessage}")
                : response.Data;
        }

        public async Task DeleteUserAsync(int id)
        {
            RestRequest request = new RestRequest($"users/{id}", Method.Delete);
            RestResponse response = await this._client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException($"Failed to delete user: {response.ErrorMessage}");
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            RestRequest restRequest = new RestRequest("auth/login", Method.Post)
                .AddJsonBody(request);

            RestResponse<LoginResponse> response = await this._client.ExecuteAsync<LoginResponse>(restRequest);

            return !response.IsSuccessful || response.Data is null
                ? throw new InvalidOperationException($"Failed to login: {response.ErrorMessage}")
                : response.Data;
        }
    }
}
