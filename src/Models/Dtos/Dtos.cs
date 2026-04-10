using System;

namespace ShoppingCart.Models.Dtos
{
    public record ProductDto(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int SalesVolume,
        string? ImageUrl,
        DateTime CreatedTime,
        string CategoryName,
        string? Material,
        string? Color
    );

    public record CategoryDto(
        int Id,
        string Name,
        string? Description
    );
}
