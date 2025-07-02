namespace SemanticAPITestAutomation.Core.Models
{
    /// <summary>
    /// Result of validating expected vs actual function invocations
    /// </summary>
    public record InvocationValidationResult(
        bool IsValid,
        IReadOnlyList<string> ExpectedButNotInvoked,
        IReadOnlyList<string> InvokedButNotExpected,
        IReadOnlyList<string> SuccessfullyInvoked
    );
}
