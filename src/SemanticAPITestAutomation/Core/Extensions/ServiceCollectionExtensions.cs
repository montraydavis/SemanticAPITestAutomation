namespace SemanticAPITestAutomation.Core.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.SemanticKernel;

    using SemanticAPITestAutomation;
    using SemanticAPITestAutomation.Core;
    using SemanticAPITestAutomation.Core.Services;

    /// <summary>
    /// Extension methods for configuring AI services in dependency injection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AI API Test Automation services to the dependency injection container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="apiKey">OpenAI API key for Semantic Kernel</param>
        /// <param name="modelId">Optional model ID (defaults to gpt-4)</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddAIApiTestAutomation(this IServiceCollection services, string apiKey, string modelId = "gpt-4")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));
            }

            // Register tracking service as singleton to persist across test runs
            services.AddSingleton<IKernelFunctionTrackingService, KernelFunctionTrackingService>();
            services.AddOpenAIChatCompletion(modelId, apiKey);

            // Configure Semantic Kernel
            services.AddSingleton(serviceProvider =>
            {
                Kernel kernel = new Kernel(services: serviceProvider);

                // Register test functions from the repository tests directly
                kernel.Plugins.AddFromType<FakeStoreApiRepositoryTests>("FakeStoreApiTests", serviceProvider: serviceProvider);

                return kernel;
            });

            // Register AI service
            services.AddTransient<IAIApiTestAutomationService, AIApiTestAutomationService>();

            return services;
        }
    }
}