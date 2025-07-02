namespace SemanticAPITestAutomation.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using SemanticAPITestAutomation.Core.Extensions;

    public abstract class TestBase
    {
        protected ServiceProvider ServiceProvider { get; private set; } = null!;
        protected IServiceScope Scope { get; private set; } = null!;
        protected ILogger Logger { get; private set; } = null!;

        protected IKernelFunctionTrackingService? _trackingService;

        public TestBase()
        {

        }

        public TestBase(IKernelFunctionTrackingService trackingService)
        {
            this._trackingService = trackingService;
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string testApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                    ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set");

            ServiceCollection services = new ServiceCollection();
            services.AddAIApiTestAutomation(testApiKey, "gpt-4o-mini");
            services.AddLogging(builder =>
                builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
            this.ConfigureServices(services);
            this.ServiceProvider = services.BuildServiceProvider();

            this._trackingService = this.ServiceProvider.GetRequiredService<IKernelFunctionTrackingService>();
        }

        [SetUp]
        public void SetUp()
        {
            this.Scope = this.ServiceProvider.CreateScope();
            this.Logger = this.Scope.ServiceProvider.GetRequiredService<ILogger<TestBase>>();
            this.OnSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            this.OnTearDown();
            this.Scope?.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.OnOneTimeTearDown();
            this.ServiceProvider?.Dispose();
        }

        protected abstract void ConfigureServices(IServiceCollection services);
        protected virtual void OnSetUp() { }
        protected virtual void OnTearDown() { }
        protected virtual void OnOneTimeTearDown() { }

        protected T GetService<T>() where T : notnull => this.Scope.ServiceProvider.GetRequiredService<T>();
        protected T? GetOptionalService<T>() => this.Scope.ServiceProvider.GetService<T>();
    }
}