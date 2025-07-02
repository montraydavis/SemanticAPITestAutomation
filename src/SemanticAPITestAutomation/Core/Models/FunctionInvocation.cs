namespace SemanticAPITestAutomation.Core.Models
{
    /// <summary>
    /// Records details of a kernel function invocation
    /// </summary>
    public record FunctionInvocation(
        string FunctionName,
        DateTime InvokedAt,
        Dictionary<string, object>? Parameters = null
    );
}
