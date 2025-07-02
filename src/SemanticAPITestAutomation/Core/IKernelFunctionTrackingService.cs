namespace SemanticAPITestAutomation.Core
{
    using SemanticAPITestAutomation.Core.Models;

    /// <summary>
    /// Tracks kernel function invocations for test validation
    /// </summary>
    public interface IKernelFunctionTrackingService
    {
        /// <summary>
        /// Records that a kernel function was invoked
        /// </summary>
        /// <param name="functionName">Name of the invoked function</param>
        /// <param name="parameters">Optional parameters passed to the function</param>
        void RecordInvocation(string functionName, Dictionary<string, object>? parameters = null);

        /// <summary>
        /// Gets all recorded invocations
        /// </summary>
        /// <returns>List of function invocations</returns>
        IReadOnlyList<FunctionInvocation> GetInvocations();

        /// <summary>
        /// Checks if a specific function was invoked
        /// </summary>
        /// <param name="functionName">Name of the function to check</param>
        /// <returns>True if function was invoked</returns>
        bool WasInvoked(string functionName);

        /// <summary>
        /// Gets the count of times a function was invoked
        /// </summary>
        /// <param name="functionName">Name of the function</param>
        /// <returns>Number of invocations</returns>
        int GetInvocationCount(string functionName);

        /// <summary>
        /// Clears all recorded invocations
        /// </summary>
        void ClearInvocations();

        /// <summary>
        /// Validates that expected functions were invoked
        /// </summary>
        /// <param name="expectedFunctions">List of expected function names</param>
        /// <returns>Validation result</returns>
        InvocationValidationResult ValidateExpectedInvocations(IEnumerable<string> expectedFunctions);
    }
}
