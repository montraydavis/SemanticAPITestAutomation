namespace SemanticAPITestAutomation
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using SemanticAPITestAutomation.Core;
    using SemanticAPITestAutomation.Core.Extensions;
    using SemanticAPITestAutomation.Core.Models;

    public class AIApiTestAutomationServiceTests : TestBase
    {

        [Test, Description("Verify AI can generate strategy for GetAllProducts test and invoke correct function")]
        public async Task GetAllProducts_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "GetAllProducts_ShouldReturnProducts",
                "Get all products",
                "products",
                expectedFunctions: ["GetAllProducts_ShouldReturnProducts"]
            );
        }

        [Test, Description("Verify AI can generate strategy for GetProductById with valid ID test and invoke correct function")]
        public async Task GetProductById_WithValidId_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "GetProductById_WithValidId_ShouldReturnProduct",
                "Get product by ID.",
                "product",
                expectedFunctions: ["GetProductById_WithValidId_ShouldReturnProduct"]
            );
        }

        [Test, Description("Verify AI can generate strategy for GetProductById with invalid ID test and invoke correct function")]
        public async Task GetProductById_WithInvalidId_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "GetProductById_WithInvalidId_ShouldReturnNull",
                "Get product using invalid ID.",
                "invalid",
                expectedFunctions: ["GetProductById_WithInvalidId_ShouldReturnNull"]
            );
        }

        [Test, Description("Verify AI can generate strategy for CreateProduct test and invoke correct function")]
        public async Task CreateProduct_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "CreateProduct_ShouldReturnCreatedProduct",
                "Create a product.",
                "create",
                expectedFunctions: ["CreateProduct_ShouldReturnCreatedProduct"]
            );
        }

        [Test, Description("Verify AI can generate comprehensive strategy for multiple product operations")]
        public async Task ProductOperations_Comprehensive_ShouldInvokeMultipleFunctions()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "ProductOperations_Comprehensive",
                "Generate a comprehensive test strategy for all product-related operations including retrieving all products, getting specific products by ID, handling invalid IDs, and creating new products.",
                "comprehensive",
                expectedFunctions: [
                    "GetAllProducts_ShouldReturnProducts",
                    "GetProductById_WithValidId_ShouldReturnProduct",
                    "GetProductById_WithInvalidId_ShouldReturnNull",
                    "CreateProduct_ShouldReturnCreatedProduct"
                ]
            );
        }

        [Test, Description("Verify AI can generate strategy for GetAllCarts test and invoke correct function")]
        public async Task GetAllCarts_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "GetAllCarts_ShouldReturnCarts",
                "Get all carts",
                "carts",
                expectedFunctions: ["GetAllCarts_ShouldReturnCarts"]
            );
        }

        [Test, Description("Verify AI can generate strategy for GetAllUsers test and invoke correct function")]
        public async Task GetAllUsers_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "GetAllUsers_ShouldReturnUsers",
                "Get all users.",
                "users",
                expectedFunctions: ["GetAllUsers_ShouldReturnUsers"]
            );
        }

        [Test, Description("Verify AI can generate strategy for Login test and invoke correct function")]
        public async Task Login_WithValidCredentials_ShouldReturnValidStrategyAndInvokeFunction()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "Login_WithValidCredentials_ShouldReturnToken",
                "Login with valid credentials.",
                "token",
                expectedFunctions: ["Login_WithValidCredentials_ShouldReturnToken"]
            );
        }

        [Test, Description("Verify AI can generate comprehensive strategy for full API test suite")]
        public async Task FullApiTestSuite_ShouldInvokeAllAvailableFunctions()
        {
            await this.ValidateAIStrategyWithFunctionInvocation(
                "FullApiTestSuite_Comprehensive",
                "Generate a comprehensive test strategy that covers all available API endpoints including products, carts, users, and authentication. Ensure proper test coverage across all CRUD operations and error scenarios.",
                "comprehensive",
                expectedFunctions: [
                    "GetAllProducts_ShouldReturnProducts",
                    "GetProductById_WithValidId_ShouldReturnProduct",
                    "GetProductById_WithInvalidId_ShouldReturnNull",
                    "CreateProduct_ShouldReturnCreatedProduct",
                    "GetAllCarts_ShouldReturnCarts",
                    "GetAllUsers_ShouldReturnUsers",
                    "Login_WithValidCredentials_ShouldReturnToken"
                ]
            );
        }

        [Test, Description("Verify AI service handles empty instructions gracefully")]
        public void DetermineTestExecutionStrategy_WithEmptyInstructions_ShouldThrowArgumentException()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: AI Service - Empty Instructions Handling");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining AI service...");
                IAIApiTestAutomationService aiService = this.GetService<IAIApiTestAutomationService>();
                this.Logger.LogInformation("   ├─ ✅ AI service obtained");
                this.Logger.LogInformation("   └─ 🚫 Empty instructions prepared for error testing");

                // Act & Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT & ASSERT PHASE");
                this.Logger.LogInformation("   ├─ Calling DetermineTestExecutionStrategyAsync() with empty string...");

                ArgumentException? exception = Assert.ThrowsAsync<ArgumentException>(
                    () => aiService.DetermineTestExecutionStrategyAsync(string.Empty));

                this.Logger.LogInformation("   ├─ ✅ Expected ArgumentException was thrown");
                this.Logger.LogInformation("   ├─ Exception message: '{Message}'", exception.Message);
                this.Logger.LogInformation("   └─ ✅ Error handling validation passed");

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex) when (ex is not AssertionException)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Exception Type: {ExceptionType}", ex.GetType().Name);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        [Test, Description("Verify AI service can handle cancellation requests")]
        public void DetermineTestExecutionStrategy_WithCancellation_ShouldThrowOperationCanceledException()
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: AI Service - Cancellation Handling");
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining AI service...");
                IAIApiTestAutomationService aiService = this.GetService<IAIApiTestAutomationService>();

                using CancellationTokenSource cts = new CancellationTokenSource();
                const string instructions = "Test all API endpoints comprehensively";

                this.Logger.LogInformation("   ├─ ✅ AI service obtained");
                this.Logger.LogInformation("   ├─ 🎛️ Cancellation token source created");
                this.Logger.LogInformation("   └─ 📝 Test instructions: '{Instructions}'", instructions);

                // Act & Assert
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT & ASSERT PHASE");
                this.Logger.LogInformation("   ├─ Cancelling token immediately...");
                cts.Cancel();

                this.Logger.LogInformation("   ├─ Calling DetermineTestExecutionStrategyAsync() with cancelled token...");

                OperationCanceledException? exception = Assert.ThrowsAsync<OperationCanceledException>(
                    () => aiService.DetermineTestExecutionStrategyAsync(instructions, cts.Token));

                this.Logger.LogInformation("   ├─ ✅ Expected OperationCanceledException was thrown");
                this.Logger.LogInformation("   ├─ Exception message: '{Message}'", exception.Message);
                this.Logger.LogInformation("   └─ ✅ Cancellation handling validation passed");

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
            }
            catch (Exception ex) when (ex is not AssertionException)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Exception Type: {ExceptionType}", ex.GetType().Name);
                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        private async Task ValidateAIStrategyWithFunctionInvocation(
            string testName,
            string instructions,
            string expectedKeyword,
            string[] expectedFunctions)
        {
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            this.Logger.LogInformation("🧪 TEST STARTED: AI Service - {TestName}", testName);
            this.Logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                // Arrange
                this.Logger.LogInformation("📋 ARRANGE PHASE");
                this.Logger.LogInformation("   ├─ Obtaining AI service...");
                IAIApiTestAutomationService aiService = this.GetService<IAIApiTestAutomationService>();

                this.Logger.LogInformation("   ├─ Obtaining tracking service...");
                IKernelFunctionTrackingService trackingService = this.GetService<IKernelFunctionTrackingService>();

                this.Logger.LogInformation("   ├─ Clearing previous invocations...");
                trackingService.ClearInvocations();

                this.Logger.LogInformation("   ├─ ✅ Services obtained successfully");
                this.Logger.LogInformation("   ├─ 📝 Test instructions prepared:");
                this.Logger.LogInformation("       └─ '{Instructions}'", instructions);
                this.Logger.LogInformation("   └─ 🎯 Expected functions: [{ExpectedFunctions}]", string.Join(", ", expectedFunctions));

                // Act
                this.Logger.LogInformation("");
                this.Logger.LogInformation("⚡ ACT PHASE");
                this.Logger.LogInformation("   ├─ Calling DetermineTestExecutionStrategyAsync()...");

                string strategy = await aiService.DetermineTestExecutionStrategyAsync(instructions);

                this.Logger.LogInformation("   ├─ ✅ AI analysis completed successfully");
                this.Logger.LogInformation("   ├─ Strategy length: {StrategyLength} characters", strategy.Length);
                this.Logger.LogInformation("   └─ Strategy preview: '{Preview}...'",
                    strategy.Length > 100 ? strategy[..100] : strategy);

                // Assert Strategy Content
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE - Strategy Content");

                this.Logger.LogDebug("   ├─ Validating strategy is not null or empty...");
                Assert.That(strategy, Is.Not.Null.And.Not.Empty);
                this.Logger.LogInformation("   ├─ ✅ Strategy content validation passed");

                this.Logger.LogDebug("   ├─ Validating strategy contains expected keyword '{ExpectedKeyword}'...", expectedKeyword);
                Assert.That(strategy.ToLowerInvariant(), Does.Contain(expectedKeyword.ToLowerInvariant()));
                this.Logger.LogInformation("   ├─ ✅ Keyword validation passed");

                this.Logger.LogDebug("   ├─ Validating strategy has reasonable length...");
                Assert.That(strategy.Length, Is.GreaterThan(50));
                this.Logger.LogInformation("   └─ ✅ Strategy length validation passed: {Length} characters", strategy.Length);

                // Assert Function Invocations
                this.Logger.LogInformation("");
                this.Logger.LogInformation("✔️ ASSERT PHASE - Function Invocations");

                this.Logger.LogInformation("   ├─ Retrieving recorded invocations...");
                IReadOnlyList<FunctionInvocation> actualInvocations = trackingService.GetInvocations();
                this.Logger.LogInformation("   ├─ Recorded invocations: {InvocationCount}", actualInvocations.Count);

                foreach (FunctionInvocation invocation in actualInvocations)
                {
                    this.Logger.LogInformation("       ├─ '{FunctionName}' at {InvokedAt:HH:mm:ss.fff}",
                        invocation.FunctionName, invocation.InvokedAt);
                }

                this.Logger.LogInformation("   ├─ Validating expected function invocations...");
                InvocationValidationResult validationResult = trackingService.ValidateExpectedInvocations(expectedFunctions);

                this.Logger.LogInformation("   ├─ Validation Result:");
                this.Logger.LogInformation("       ├─ Is Valid: {IsValid}", validationResult.IsValid);
                this.Logger.LogInformation("       ├─ Successfully Invoked: [{SuccessfullyInvoked}]",
                    string.Join(", ", validationResult.SuccessfullyInvoked));

                if (validationResult.ExpectedButNotInvoked.Count > 0)
                {
                    this.Logger.LogWarning("       ├─ Expected but NOT invoked: [{ExpectedButNotInvoked}]",
                        string.Join(", ", validationResult.ExpectedButNotInvoked));
                }

                if (validationResult.InvokedButNotExpected.Count > 0)
                {
                    this.Logger.LogInformation("       ├─ Invoked but not expected: [{InvokedButNotExpected}]",
                        string.Join(", ", validationResult.InvokedButNotExpected));
                }

                // Core validation: all expected functions must be invoked
                Assert.That(validationResult.IsValid, Is.True,
                    $"Expected functions were not invoked: {string.Join(", ", validationResult.ExpectedButNotInvoked)}");

                this.Logger.LogInformation("   ├─ ✅ All expected functions were successfully invoked");

                // Verify individual function invocations
                foreach (string expectedFunction in expectedFunctions)
                {
                    this.Logger.LogDebug("   ├─ Verifying '{ExpectedFunction}' was invoked...", expectedFunction);
                    Assert.That(trackingService.WasInvoked(expectedFunction), Is.True,
                        $"Expected function '{expectedFunction}' was not invoked");

                    int invocationCount = trackingService.GetInvocationCount(expectedFunction);
                    this.Logger.LogInformation("       └─ ✅ '{ExpectedFunction}' invoked {Count} time(s)",
                        expectedFunction, invocationCount);
                }

                this.Logger.LogInformation("   └─ ✅ Function invocation validation completed successfully");

                this.Logger.LogInformation("");
                this.Logger.LogInformation("🎉 TEST COMPLETED SUCCESSFULLY");
                this.Logger.LogInformation("   ├─ Strategy validation: ✅ Passed");
                this.Logger.LogInformation("   ├─ Function invocation validation: ✅ Passed");
                this.Logger.LogInformation("   └─ Expected vs Actual functions: ✅ Match");
            }
            catch (Exception ex)
            {
                this.Logger.LogError("❌ TEST FAILED: {ErrorMessage}", ex.Message);
                this.Logger.LogError("   Exception Type: {ExceptionType}", ex.GetType().Name);
                if (ex.InnerException != null)
                {
                    this.Logger.LogError("   Inner Exception: {InnerException}", ex.InnerException.Message);
                }

                throw;
            }
            finally
            {
                this.Logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            // Note: In real tests, you would use a test API key or mock the service
            // For demonstration purposes, this shows the configuration pattern
            string testApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                    ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set");

            services.AddAIApiTestAutomation(testApiKey, "gpt-4.1-nano");
        }
    }
}