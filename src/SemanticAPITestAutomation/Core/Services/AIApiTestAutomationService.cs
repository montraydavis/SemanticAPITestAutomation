namespace SemanticAPITestAutomation.Core.Services
{
    using Microsoft.Extensions.Logging;
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.Connectors.OpenAI;

    using SemanticAPITestAutomation.Core;

    /// <summary>
    /// AI-powered service for determining API test execution strategies using semantic reasoning
    /// </summary>
    internal sealed class AIApiTestAutomationService : IAIApiTestAutomationService
    {
        private readonly Kernel _kernel;
        private readonly ILogger<AIApiTestAutomationService> _logger;

        public AIApiTestAutomationService(Kernel kernel, ILogger<AIApiTestAutomationService> logger)
        {
            this._kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> DetermineTestExecutionStrategyAsync(string instructions, CancellationToken cancellationToken = default)
        {
            this._logger.LogInformation("═══════════════════════════════════════════════════════");
            this._logger.LogInformation("🤖 AI TEST STRATEGY ANALYSIS STARTED");
            this._logger.LogInformation("═══════════════════════════════════════════════════════");

            try
            {
                await this.ValidateInputAsync(instructions, cancellationToken);

                string prompt = this.BuildStructuredPrompt(instructions);
                OpenAIPromptExecutionSettings executionSettings = CreatePromptExecutionSettings();
                KernelArguments kernelArgs = new KernelArguments(executionSettings);

                return await this.ExecuteSemanticAnalysisAsync(prompt, kernelArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogError("❌ AI TEST STRATEGY ANALYSIS FAILED: {ErrorMessage}", ex.Message);
                this._logger.LogError("   Exception Type: {ExceptionType}", ex.GetType().Name);
                this._logger.LogError("   Stack Trace: {StackTrace}", ex.StackTrace);
                throw;
            }
            finally
            {
                this._logger.LogInformation("═══════════════════════════════════════════════════════");
            }
        }

        private async Task ValidateInputAsync(string instructions, CancellationToken cancellationToken)
        {
            this._logger.LogInformation("📋 INPUT VALIDATION PHASE");

            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(instructions))
            {
                this._logger.LogError("   ❌ Invalid input: Instructions cannot be null or empty");
                throw new ArgumentException("Instructions cannot be null or empty.", nameof(instructions));
            }

            if (instructions.Length > 5000)
            {
                this._logger.LogWarning("   ⚠️ Instructions are quite long: {Length} characters", instructions.Length);
            }

            this._logger.LogInformation("   ├─ Instructions length: {Length} characters", instructions.Length);
            this._logger.LogInformation("   ├─ Instructions preview: '{Preview}...'",
                instructions.Length > 100 ? instructions[..100] : instructions);
            this._logger.LogInformation("   └─ ✅ Input validation passed");

            await Task.CompletedTask; // Placeholder for async validation if needed
        }

        private string BuildStructuredPrompt(string instructions)
        {
            this._logger.LogInformation("");
            this._logger.LogInformation("🏗️ PROMPT CONSTRUCTION PHASE");

            string prompt = $$$"""
                # API Test Execution Strategy Analyzer

                You are an expert API test automation strategist. Your role is to analyze user requirements and determine the optimal set of API tests to execute for the FakeStore API.

                ## Available API Test Functions
                You have access to the following KernelFunction-enabled API tests:

                ### Product Tests
                - `GetAllProducts_ShouldReturnProducts`: Retrieve all products from the FakeStore API
                - `GetProductById_WithValidId_ShouldReturnProduct`: Get a specific product by ID
                - `GetProductById_WithInvalidId_ShouldReturnNull`: Test error handling for invalid product IDs
                - `CreateProduct_ShouldReturnCreatedProduct`: Create a new product via POST request

                ### Cart Tests  
                - `GetAllCarts_ShouldReturnCarts`: Retrieve all shopping carts
                - `GetCartByIdAsync`: Get a specific cart by ID (if needed)

                ### User Tests
                - `GetAllUsers_ShouldReturnUsers`: Retrieve all users from the system
                - `GetUserByIdAsync`: Get a specific user by ID (if needed)

                ### Authentication Tests
                - `Login_WithValidCredentials_ShouldReturnToken`: Test user authentication and JWT token generation

                ## Analysis Framework

                ### 1. Requirement Analysis
                - Parse the user instructions to identify:
                  - **Scope**: Which API endpoints are relevant?
                  - **Test Types**: CRUD operations, error scenarios, authentication?
                  - **Priority**: Critical vs. nice-to-have tests
                  - **Dependencies**: Which tests should run in sequence?

                ### 2. Test Selection Strategy
                - **Comprehensive Coverage**: For broad requirements, include all relevant tests
                - **Targeted Testing**: For specific scenarios, focus on relevant functions
                - **Error Scenarios**: Always include negative test cases when appropriate
                - **Authentication Flow**: Include login tests when user operations are involved

                ### 3. Execution Sequence
                - **Foundation First**: Authentication and basic GET operations
                - **Core Functionality**: Primary business operations
                - **Edge Cases**: Error handling and boundary conditions
                - **Integration**: Cross-functional scenarios

                ## Response Format

                Provide your analysis in this structured format:

                ```
                ## Test Strategy Analysis

                ### Requirements Summary
                [Brief summary of what the user wants to test]

                ### Recommended Test Functions
                1. **[Test Function Name]**
                   - **Purpose**: [Why this test is needed]
                   - **Priority**: [High/Medium/Low]
                   - **Dependencies**: [Any prerequisite tests]

                [Repeat for each recommended test]

                ### Execution Sequence
                1. [First test to run]
                2. [Second test to run]
                [...continue sequence...]

                ### Expected Outcomes
                - [What successful execution should demonstrate]
                - [Potential failure scenarios to watch for]

                ### Risk Assessment
                - [Potential issues or limitations]
                - [Mitigation strategies]
                ```

                ## User Instructions
                {{{instructions}}}

                ## Analysis Request
                Based on the user instructions above, analyze the requirements and invoke the required tools.
                """;

            this._logger.LogInformation("   ├─ Prompt structure: Multi-section analysis framework");
            this._logger.LogInformation("   ├─ Available test functions: 8 core API test methods");
            this._logger.LogInformation("   ├─ Analysis framework: 3-phase approach (Requirements, Selection, Sequence)");
            this._logger.LogInformation("   ├─ Prompt length: {PromptLength} characters", prompt.Length);
            this._logger.LogInformation("   └─ ✅ Structured prompt constructed successfully");

            return prompt;
        }

        private static OpenAIPromptExecutionSettings CreatePromptExecutionSettings()
        {
            return new OpenAIPromptExecutionSettings
            {
                Temperature = 0.3f, // Lower temperature for more consistent analysis
                MaxTokens = 2000,   // Sufficient for detailed analysis
                //TopP = 0.9f,        // Focused responses
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };
        }

        private async Task<string> ExecuteSemanticAnalysisAsync(string prompt, KernelArguments kernelArgs, CancellationToken cancellationToken)
        {
            this._logger.LogInformation("");
            this._logger.LogInformation("🧠 SEMANTIC ANALYSIS EXECUTION PHASE");

            this._logger.LogInformation("   ├─ Invoking AI kernel with structured prompt...");
            this._logger.LogDebug("   ├─ Temperature: {Temperature}", 0.3f);
            this._logger.LogDebug("   ├─ Max Tokens: {MaxTokens}", 2000);
            this._logger.LogDebug("   ├─ Function Choice: Auto");

            FunctionResult testInvocationRequest = await this._kernel.InvokePromptAsync(prompt, kernelArgs, cancellationToken: cancellationToken);

            this._logger.LogInformation("   ├─ ✅ AI analysis completed successfully");

            string? testInvocationResponse = testInvocationRequest.GetValue<string>();

            if (string.IsNullOrWhiteSpace(testInvocationResponse))
            {
                this._logger.LogError("   ❌ AI returned empty or null response");
                throw new InvalidOperationException("AI analysis failed to generate a valid test execution strategy. The response was empty or null.");
            }

            this._logger.LogInformation("   ├─ Response length: {ResponseLength} characters", testInvocationResponse.Length);
            this._logger.LogInformation("   ├─ Response preview: '{Preview}...'",
                testInvocationResponse.Length > 150 ? testInvocationResponse[..150] : testInvocationResponse);
            this._logger.LogInformation("   └─ ✅ Valid test strategy generated");

            this._logger.LogInformation("");
            this._logger.LogInformation("🎉 AI TEST STRATEGY ANALYSIS COMPLETED SUCCESSFULLY");

            return testInvocationResponse;
        }
    }
}
