namespace SemanticAPITestAutomation.Core.Services
{
    using System.Collections.Concurrent;

    using SemanticAPITestAutomation.Core;
    using SemanticAPITestAutomation.Core.Models;

    /// <summary>
    /// Thread-safe implementation of kernel function invocation tracking
    /// </summary>
    internal sealed class KernelFunctionTrackingService : IKernelFunctionTrackingService
    {
        private readonly ConcurrentBag<FunctionInvocation> _invocations = new();

        public void RecordInvocation(string functionName, Dictionary<string, object>? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(functionName))
            {
                throw new ArgumentException("Function name cannot be null or empty", nameof(functionName));
            }

            FunctionInvocation invocation = new FunctionInvocation(
                functionName,
                DateTime.UtcNow,
                parameters?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            );

            this._invocations.Add(invocation);
        }

        public IReadOnlyList<FunctionInvocation> GetInvocations() =>
            this._invocations.OrderBy(i => i.InvokedAt).ToList().AsReadOnly();

        public bool WasInvoked(string functionName) =>
            this._invocations.Any(i => string.Equals(i.FunctionName, functionName, StringComparison.OrdinalIgnoreCase));

        public int GetInvocationCount(string functionName) =>
            this._invocations.Count(i => string.Equals(i.FunctionName, functionName, StringComparison.OrdinalIgnoreCase));

        public void ClearInvocations()
        {
            while (this._invocations.TryTake(out _))
            {
            }
        }

        public InvocationValidationResult ValidateExpectedInvocations(IEnumerable<string> expectedFunctions)
        {
            HashSet<string> expected = expectedFunctions.ToHashSet(StringComparer.OrdinalIgnoreCase);
            HashSet<string> invoked = this._invocations.Select(i => i.FunctionName).ToHashSet(StringComparer.OrdinalIgnoreCase);

            System.Collections.ObjectModel.ReadOnlyCollection<string> expectedButNotInvoked = expected.Except(invoked).ToList().AsReadOnly();
            System.Collections.ObjectModel.ReadOnlyCollection<string> invokedButNotExpected = invoked.Except(expected).ToList().AsReadOnly();
            System.Collections.ObjectModel.ReadOnlyCollection<string> successfullyInvoked = expected.Intersect(invoked).ToList().AsReadOnly();

            bool isValid = expectedButNotInvoked.Count == 0;

            return new InvocationValidationResult(
                isValid,
                expectedButNotInvoked,
                invokedButNotExpected,
                successfullyInvoked
            );
        }
    }
}
