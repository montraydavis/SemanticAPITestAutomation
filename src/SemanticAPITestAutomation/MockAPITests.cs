namespace SemanticAPITestAutomation
{

    using global::SemanticAPITestAutomation.Core;
    using global::SemanticAPITestAutomation.Core.Models;
    using global::SemanticAPITestAutomation.Core.Repositories;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework.Internal;

    /// <summary>
    /// Unit tests for API repository using Moq for dependency isolation
    /// </summary>
    public class MockAPITests : TestBase
    {
        private Mock<IFakeStoreApiRepository> _mockRepository = null!;

        protected override void ConfigureServices(IServiceCollection services)
        {
            // Create mock repository instance
            this._mockRepository = new Mock<IFakeStoreApiRepository>();

            // Register the mock object in DI container
            services.AddSingleton(this._mockRepository.Object);
        }

        protected override void OnSetUp()
        {
            this._mockRepository.Reset();
        }

        [Test, Description("Verify GetAllProducts returns expected product collection")]
        public async Task GetAllProducts_WhenCalled_ShouldReturnMockedProducts()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: GetAllProducts_WhenCalled_ShouldReturnMockedProducts");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Setting up mock repository...");

                System.Collections.ObjectModel.ReadOnlyCollection<Product> expectedProducts = new List<Product>
                {
                    new(1, "Test Product 1", 19.99m, "Test Description 1", "electronics", "test-image-1.jpg"),
                    new(2, "Test Product 2", 29.99m, "Test Description 2", "clothing", "test-image-2.jpg")
                }.AsReadOnly();

                this._mockRepository
                    .Setup(r => r.GetAllProductsAsync())
                    .ReturnsAsync(expectedProducts);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   ├─ ✅ Mock setup completed");
                this.Logger.LogInformation("   └─ 📦 Expected products: {Count} items", expectedProducts.Count);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling GetAllProductsAsync()...");

                IReadOnlyList<Product> result = await repository.GetAllProductsAsync();

                this.Logger.LogInformation("   └─ ✅ Method executed successfully");

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                this.Logger.LogDebug("   ├─ Verifying result is not null...");
                Assert.That(result, Is.Not.Null);
                this.Logger.LogInformation("   ├─ ✅ Result validation passed");

                this.Logger.LogDebug("   ├─ Verifying product count...");
                Assert.That(result.Count, Is.EqualTo(expectedProducts.Count));
                this.Logger.LogInformation("   ├─ ✅ Count validation passed: {Expected} = {Actual}",
                    expectedProducts.Count, result.Count);

                this.Logger.LogDebug("   ├─ Verifying first product properties...");
                Assert.That(result[0].Title, Is.EqualTo(expectedProducts[0].Title));
                Assert.That(result[0].Price, Is.EqualTo(expectedProducts[0].Price));
                this.Logger.LogInformation("   ├─ ✅ First product validation passed");

                this.Logger.LogDebug("   ├─ Verifying mock was called exactly once...");
                this._mockRepository.Verify(r => r.GetAllProductsAsync(), Times.Once);
                this.Logger.LogInformation("   └─ ✅ Mock verification passed");

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Exception Type: {ExceptionType}", ex.GetType().Name);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify GetProductById returns correct product for valid ID")]
        public async Task GetProductById_WithValidId_ShouldReturnMockedProduct()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: GetProductById_WithValidId_ShouldReturnMockedProduct");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                const int productId = 42;
                Product expectedProduct = new Product(productId, "Mocked Product", 99.99m, "Mocked Description", "test", "mock.jpg");

                this._mockRepository
                    .Setup(r => r.GetProductByIdAsync(productId))
                    .ReturnsAsync(expectedProduct);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   ├─ ✅ Mock configured for product ID: {ProductId}", productId);
                this.Logger.LogInformation("   └─ 📦 Expected product: '{Title}'", expectedProduct.Title);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                Product? result = await repository.GetProductByIdAsync(productId);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(productId));
                Assert.That(result.Title, Is.EqualTo(expectedProduct.Title));
                Assert.That(result.Price, Is.EqualTo(expectedProduct.Price));

                this._mockRepository.Verify(r => r.GetProductByIdAsync(productId), Times.Once);

                this.Logger.LogInformation("   └─ ✅ All assertions passed");
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify GetProductById returns null for invalid ID")]
        public async Task GetProductById_WithInvalidId_ShouldReturnNull()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: GetProductById_WithInvalidId_ShouldReturnNull");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                const int invalidId = 999999;

                this._mockRepository
                    .Setup(r => r.GetProductByIdAsync(invalidId))
                    .ReturnsAsync((Product?)null);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   └─ 🚫 Mock configured to return null for ID: {InvalidId}", invalidId);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                Product? result = await repository.GetProductByIdAsync(invalidId);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                Assert.That(result, Is.Null);
                this._mockRepository.Verify(r => r.GetProductByIdAsync(invalidId), Times.Once);

                this.Logger.LogInformation("   └─ ✅ Null validation passed");
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify CreateProduct returns created product with assigned ID")]
        public async Task CreateProduct_WithValidProduct_ShouldReturnCreatedProduct()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: CreateProduct_WithValidProduct_ShouldReturnCreatedProduct");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                Product inputProduct = new Product(0, "New Product", 49.99m, "New Description", "electronics", "new.jpg");
                Product createdProduct = inputProduct with { Id = 123 }; // Simulate API assigning ID

                this._mockRepository
                    .Setup(r => r.CreateProductAsync(It.Is<Product>(p => p.Title == inputProduct.Title)))
                    .ReturnsAsync(createdProduct);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   ├─ 📦 Input product: '{Title}' (ID: {Id})", inputProduct.Title, inputProduct.Id);
                this.Logger.LogInformation("   └─ 🎯 Expected created product ID: {CreatedId}", createdProduct.Id);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                Product result = await repository.CreateProductAsync(inputProduct);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(createdProduct.Id));
                Assert.That(result.Title, Is.EqualTo(inputProduct.Title));
                Assert.That(result.Price, Is.EqualTo(inputProduct.Price));

                this._mockRepository.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Once);

                this.Logger.LogInformation("   ├─ ✅ Created product ID: {ActualId}", result.Id);
                this.Logger.LogInformation("   └─ ✅ All product properties validated");
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify CreateProduct throws exception when repository fails")]
        public void CreateProduct_WhenRepositoryFails_ShouldThrowException()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: CreateProduct_WhenRepositoryFails_ShouldThrowException");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                Product inputProduct = new Product(0, "Failed Product", 29.99m, "Test Description", "test", "test.jpg");
                InvalidOperationException expectedException = new InvalidOperationException("Repository operation failed");

                this._mockRepository
                    .Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                    .ThrowsAsync(expectedException);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   └─ 💥 Mock configured to throw exception");

                // Act & Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT & ASSERT PHASE");

                InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                    () => repository.CreateProductAsync(inputProduct));

                Assert.That(exception!.Message, Is.EqualTo(expectedException.Message));
                this._mockRepository.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Once);

                this.Logger.LogInformation("   ├─ ✅ Expected exception thrown: {ExceptionType}", exception.GetType().Name);
                this.Logger.LogInformation("   └─ ✅ Exception message validated: '{Message}'", exception.Message);
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex) when (ex is not AssertionException)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify GetAllUsers returns mocked user collection")]
        public async Task GetAllUsers_WhenCalled_ShouldReturnMockedUsers()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: GetAllUsers_WhenCalled_ShouldReturnMockedUsers");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                System.Collections.ObjectModel.ReadOnlyCollection<User> expectedUsers = new List<User>
                {
                    new(1, "testuser1", "test1@example.com", "password123"),
                    new(2, "testuser2", "test2@example.com", "password456")
                }.AsReadOnly();

                this._mockRepository
                    .Setup(r => r.GetAllUsersAsync())
                    .ReturnsAsync(expectedUsers);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   └─ 👥 Expected users count: {Count}", expectedUsers.Count);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                IReadOnlyList<User> result = await repository.GetAllUsersAsync();

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(expectedUsers.Count));
                Assert.That(result[0].Username, Is.EqualTo(expectedUsers[0].Username));
                Assert.That(result[1].Email, Is.EqualTo(expectedUsers[1].Email));

                this._mockRepository.Verify(r => r.GetAllUsersAsync(), Times.Once);

                this.Logger.LogInformation("   └─ ✅ User collection validated successfully");
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify Login with valid credentials returns token")]
        public async Task Login_WithValidCredentials_ShouldReturnMockedToken()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: Login_WithValidCredentials_ShouldReturnMockedToken");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                LoginRequest loginRequest = new LoginRequest("testuser", "testpass");
                LoginResponse expectedResponse = new LoginResponse("mock-jwt-token-12345");

                this._mockRepository
                    .Setup(r => r.LoginAsync(It.Is<LoginRequest>(lr =>
                        lr.Username == loginRequest.Username && lr.Password == loginRequest.Password)))
                    .ReturnsAsync(expectedResponse);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   ├─ 🔐 Login request: Username='{Username}'", loginRequest.Username);
                this.Logger.LogInformation("   └─ 🎫 Expected token: '{Token}'", expectedResponse.Token);

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                LoginResponse result = await repository.LoginAsync(loginRequest);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Token, Is.EqualTo(expectedResponse.Token));
                Assert.That(result.Token, Is.Not.Empty);

                this._mockRepository.Verify(r => r.LoginAsync(It.IsAny<LoginRequest>()), Times.Once);

                this.Logger.LogInformation("   └─ ✅ Login token validated successfully");
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify Login with invalid credentials throws exception")]
        public void Login_WithInvalidCredentials_ShouldThrowException()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: Login_WithInvalidCredentials_ShouldThrowException");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                LoginRequest invalidLoginRequest = new LoginRequest("invaliduser", "wrongpass");
                InvalidOperationException expectedException = new InvalidOperationException("Failed to login: Unauthorized");

                this._mockRepository
                    .Setup(r => r.LoginAsync(It.IsAny<LoginRequest>()))
                    .ThrowsAsync(expectedException);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   └─ 🚫 Mock configured for authentication failure");

                // Act & Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT & ASSERT PHASE");

                InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                    () => repository.LoginAsync(invalidLoginRequest));

                Assert.That(exception!.Message, Does.Contain("Failed to login"));
                this._mockRepository.Verify(r => r.LoginAsync(It.IsAny<LoginRequest>()), Times.Once);

                this.Logger.LogInformation("   └─ ✅ Authentication failure handled correctly");
                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 MOCK TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex) when (ex is not AssertionException)
            {
                this.Logger.LogError("❌ MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify complex mock interaction sequence")]
        public async Task ComplexScenario_UserCreatesProductAfterLogin_ShouldExecuteSequence()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 MOCK TEST STARTED: ComplexScenario_UserCreatesProductAfterLogin_ShouldExecuteSequence");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                LoginRequest loginRequest = new LoginRequest("productcreator", "securepass");
                LoginResponse loginResponse = new LoginResponse("auth-token-xyz");
                Product newProduct = new Product(0, "Complex Test Product", 199.99m, "Advanced test", "premium", "premium.jpg");
                Product createdProduct = newProduct with { Id = 999 };

                // Setup login mock
                this._mockRepository
                    .Setup(r => r.LoginAsync(It.IsAny<LoginRequest>()))
                    .ReturnsAsync(loginResponse);

                // Setup product creation mock
                this._mockRepository
                    .Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                    .ReturnsAsync(createdProduct);

                IFakeStoreApiRepository repository = this.GetService<IFakeStoreApiRepository>();

                this.Logger.LogInformation("   └─ 🎭 Complex scenario mocks configured");

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Step 1: Authenticating user...");
                LoginResponse authResult = await repository.LoginAsync(loginRequest);

                this.Logger.LogInformation("   ├─ Step 2: Creating product...");
                Product productResult = await repository.CreateProductAsync(newProduct);

                // Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE");

                // Verify login
                Assert.That(authResult.Token, Is.EqualTo(loginResponse.Token));
                this.Logger.LogInformation("   ├─ ✅ Authentication validated");

                // Verify product creation
                Assert.That(productResult.Id, Is.EqualTo(createdProduct.Id));
                Assert.That(productResult.Title, Is.EqualTo(newProduct.Title));
                this.Logger.LogInformation("   ├─ ✅ Product creation validated");

                // Verify call sequence
                this._mockRepository.Verify(r => r.LoginAsync(It.IsAny<LoginRequest>()), Times.Once);
                this._mockRepository.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Once);
                this.Logger.LogInformation("   └─ ✅ Method call sequence verified");

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 COMPLEX MOCK SCENARIO COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ COMPLEX MOCK TEST FAILED: {ErrorMessage}", ex.Message);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }
    }
}