namespace SemanticAPITestAutomation.Core.Models
{
    using System.Collections.Generic;

    public record Product(
        int Id,
        string Title,
        decimal Price,
        string Description,
        string Category,
        string Image
    );

    public record Cart(
        int Id,
        int UserId,
        IReadOnlyList<Product> Products
    );

    public record User(
        int Id,
        string Username,
        string Email,
        string Password
    );

    public record LoginRequest(
        string Username,
        string Password
    );

    public record LoginResponse(
        string Token
    );
}
