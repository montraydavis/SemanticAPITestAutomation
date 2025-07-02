namespace SemanticAPITestAutomation
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.SemanticKernel;

    using RestSharp;

    using SemanticAPITestAutomation.Core;
    using SemanticAPITestAutomation.Core.Models;
    using SemanticAPITestAutomation.Core.Repositories;

    public class FakeStoreApiRepositoryTests : TestBase
    {
        public FakeStoreApiRepositoryTests(IKernelFunctionTrackingService trackingService) : base(trackingService) { }
        public FakeStoreApiRepositoryTests() : base() { }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ =>
                new RestClient("https://fakestoreapi.com"));
            services.AddTransient<IFakeStoreApiRepository, FakeStoreApiRepository>();
        }

        [Test, KernelFunction, Description("Retrieve a list of all available products from the FakeStore API")]
        public async Task GetAllProducts_ShouldReturnProducts()
        {
            this._trackingService?.RecordInvocation(nameof(GetAllProducts_ShouldReturnProducts));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: GetAllProducts_ShouldReturnProducts");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();
                this.Logger.LogInformation("   └─ ✅ Repository service obtained successfully");

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling GetAllProductsAsync()...");
                IReadOnlyList<Product> products = await repository.GetAllProductsAsync();
                this.Logger.LogInformation("   └─ ✅ Received {ProductCount} products from API", products.Count);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Validating products collection is not null...");
                Assert.That(products, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Products collection is valid");

                this.Logger.LogDebug("   ├─ Validating products count > 0...");
                Assert.That(products.Count, Is.GreaterThan(0));
                this.Logger.LogInformation("   ├─ ✅ Products count validation passed: {Count} items", products.Count);

                this.Logger.LogDebug("   ├─ Validating first product has title...");
                Product firstProduct = products.First();
                Assert.That(firstProduct.Title, Is.Not.Empty);
                this.Logger.LogInformation("   └─ ✅ First product title validation passed: '{Title}'", firstProduct.Title);

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, KernelFunction, Description("Retrieve details of a specific product by ID from the FakeStore API")]
        public async Task GetProductById_WithValidId_ShouldReturnProduct()
        {
            this._trackingService?.RecordInvocation(nameof(GetProductById_WithValidId_ShouldReturnProduct));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: GetProductById_WithValidId_ShouldReturnProduct");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();
                const int productId = 1;
                this.Logger.LogInformation("   ├─ ✅ Repository service obtained");
                this.Logger.LogInformation("   └─ 🎯 Target Product ID: {ProductId}", productId);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling GetProductByIdAsync({ProductId})...", productId);
                Product? product = await repository.GetProductByIdAsync(productId);
                this.Logger.LogInformation("   └─ ✅ Product retrieved: '{ProductTitle}' (ID: {ProductId})",
                    product?.Title ?? "null", product?.Id ?? 0);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Validating product is not null...");
                Assert.That(product, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Product object validation passed");

                this.Logger.LogDebug("   ├─ Validating product ID matches request...");
                Assert.That(product.Id, Is.EqualTo(productId));
                this.Logger.LogInformation("   ├─ ✅ Product ID validation passed: Expected={ExpectedId}, Actual={ActualId}",
                    productId, product.Id);

                this.Logger.LogDebug("   ├─ Validating product title is not empty...");
                Assert.That(product.Title, Is.Not.Empty);
                this.Logger.LogInformation("   └─ ✅ Product title validation passed: '{Title}' ({Length} chars)",
                    product.Title, product.Title.Length);

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, KernelFunction, Description("Verify that requesting a non-existent product ID returns null")]
        public async Task GetProductById_WithInvalidId_ShouldReturnNull()
        {
            this._trackingService?.RecordInvocation(nameof(GetProductById_WithInvalidId_ShouldReturnNull));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: GetProductById_WithInvalidId_ShouldReturnNull");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();
                const int invalidId = 999999;
                this.Logger.LogInformation("   ├─ ✅ Repository service obtained");
                this.Logger.LogInformation("   └─ 🚫 Invalid Product ID (should not exist): {InvalidId}", invalidId);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling GetProductByIdAsync({InvalidId})...", invalidId);
                Product? product = await repository.GetProductByIdAsync(invalidId);
                this.Logger.LogInformation("   └─ ✅ API call completed, result: {Result}",
                    product == null ? "null (as expected)" : $"unexpected product: {product.Title}");

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");
                this.Logger.LogDebug("   ├─ Validating product is null for invalid ID...");
                Assert.That(product, Is.Null);
                this.Logger.LogInformation("   └─ ✅ Null validation passed - invalid ID correctly returned null");

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, KernelFunction, Description("Create a new product via POST request to the FakeStore API")]
        public async Task CreateProduct_ShouldReturnCreatedProduct()
        {
            this._trackingService?.RecordInvocation(nameof(CreateProduct_ShouldReturnCreatedProduct));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: CreateProduct_ShouldReturnCreatedProduct");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                Product newProduct = new Product(
                    Id: 0,
                    Title: "Test Product",
                    Price: 29.99m,
                    Description: "Test Description",
                    Category: "electronics",
                    Image: "https://example.com/image.jpg"
                );

                this.Logger.LogInformation("   ├─ ✅ Repository service obtained");
                this.Logger.LogInformation("   └─ 📦 Test Product Created:");
                this.Logger.LogInformation("       ├─ Title: '{Title}'", newProduct.Title);
                this.Logger.LogInformation("       ├─ Price: ${Price:F2}", newProduct.Price);
                this.Logger.LogInformation("       ├─ Category: '{Category}'", newProduct.Category);
                this.Logger.LogInformation("       └─ Description: '{Description}'", newProduct.Description);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling CreateProductAsync()...");
                Product createdProduct = await repository.CreateProductAsync(newProduct);
                this.Logger.LogInformation("   └─ ✅ Product created successfully:");
                this.Logger.LogInformation("       ├─ Assigned ID: {CreatedId}", createdProduct.Id);
                this.Logger.LogInformation("       ├─ Title: '{CreatedTitle}'", createdProduct.Title);
                this.Logger.LogInformation("       └─ Price: ${CreatedPrice:F2}", createdProduct.Price);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Validating created product is not null...");
                Assert.That(createdProduct, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Created product object validation passed");

                this.Logger.LogDebug("   ├─ Validating title matches...");
                Assert.That(createdProduct.Title, Is.EqualTo(newProduct.Title));
                this.Logger.LogInformation("   ├─ ✅ Title validation passed: '{Expected}' = '{Actual}'",
                    newProduct.Title, createdProduct.Title);

                this.Logger.LogDebug("   ├─ Validating price matches...");
                Assert.That(createdProduct.Price, Is.EqualTo(newProduct.Price));
                this.Logger.LogInformation("   └─ ✅ Price validation passed: ${Expected:F2} = ${Actual:F2}",
                    newProduct.Price, createdProduct.Price);

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, KernelFunction, Description("Retrieve a list of all shopping carts from the FakeStore API")]
        public async Task GetAllCarts_ShouldReturnCarts()
        {
            this._trackingService.RecordInvocation(nameof(GetAllCarts_ShouldReturnCarts));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: GetAllCarts_ShouldReturnCarts");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();
                this.Logger.LogInformation("   └─ ✅ Repository service obtained successfully");

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling GetAllCartsAsync()...");
                IReadOnlyList<Cart> carts = await repository.GetAllCartsAsync();
                this.Logger.LogInformation("   └─ ✅ Received {CartCount} carts from API", carts.Count);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Validating carts collection is not null...");
                Assert.That(carts, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Carts collection validation passed");

                this.Logger.LogDebug("   ├─ Validating carts count > 0...");
                Assert.That(carts.Count, Is.GreaterThan(0));
                this.Logger.LogInformation("   └─ ✅ Carts count validation passed: {Count} carts found", carts.Count);

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, KernelFunction, Description("Retrieve a list of all users from the FakeStore API")]
        public async Task GetAllUsers_ShouldReturnUsers()
        {
            this._trackingService?.RecordInvocation(nameof(GetAllUsers_ShouldReturnUsers));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: GetAllUsers_ShouldReturnUsers");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();
                this.Logger.LogInformation("   └─ ✅ Repository service obtained successfully");

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling GetAllUsersAsync()...");
                IReadOnlyList<User> users = await repository.GetAllUsersAsync();
                this.Logger.LogInformation("   └─ ✅ Received {UserCount} users from API", users.Count);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Validating users collection is not null...");
                Assert.That(users, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Users collection validation passed");

                this.Logger.LogDebug("   ├─ Validating users count > 0...");
                Assert.That(users.Count, Is.GreaterThan(0));
                this.Logger.LogInformation("   ├─ ✅ Users count validation passed: {Count} users found", users.Count);

                this.Logger.LogDebug("   ├─ Validating first user has username...");
                User firstUser = users.First();
                Assert.That(firstUser.Username, Is.Not.Empty);
                this.Logger.LogInformation("   └─ ✅ First user validation passed: '{Username}'", firstUser.Username);

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, KernelFunction, Description("Authenticate a user with valid credentials and receive a JWT token")]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            this._trackingService?.RecordInvocation(nameof(Login_WithValidCredentials_ShouldReturnToken));

            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: Login_WithValidCredentials_ShouldReturnToken");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining repository service...");
                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();
                LoginRequest loginRequest = new LoginRequest("mor_2314", "83r5^_");
                this.Logger.LogInformation("   ├─ ✅ Repository service obtained");
                this.Logger.LogInformation("   └─ 🔐 Login credentials prepared:");
                this.Logger.LogInformation("       ├─ Username: '{Username}'", loginRequest.Username);
                this.Logger.LogInformation("       └─ Password: {PasswordMask}", new string('*', loginRequest.Password.Length));

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling LoginAsync()...");
                LoginResponse response = await repository.LoginAsync(loginRequest);
                this.Logger.LogInformation("   └─ ✅ Authentication successful:");
                this.Logger.LogInformation("       ├─ Token received: {TokenLength} characters", response.Token?.Length ?? 0);
                this.Logger.LogInformation("       └─ Token preview: {TokenPreview}...",
                    response.Token?.Length > 10 ? response.Token[..10] : response.Token);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Validating login response is not null...");
                Assert.That(response, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Login response validation passed");

                this.Logger.LogDebug("   ├─ Validating token is not empty...");
                Assert.That(response.Token, Is.Not.Empty);
                this.Logger.LogInformation("   └─ ✅ Token validation passed: {TokenLength} character token received",
                    response.Token.Length);

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }
    }
}