namespace SemanticAPITestAutomation.Core
{
    /// <summary>
    /// Service for AI-powered API test automation using semantic reasoning
    /// </summary>
    public interface IAIApiTestAutomationService
    {
        /// <summary>
        /// Analyzes user instructions and determines which API tests to execute
        /// </summary>
        /// <param name="instructions">Natural language instructions describing what to test</param>
        /// <param name="cancellationToken">Cancellation token for async operations</param>
        /// <returns>AI-generated test execution strategy</returns>
        Task<string> DetermineTestExecutionStrategyAsync(string instructions, CancellationToken cancellationToken = default);
    }
}
