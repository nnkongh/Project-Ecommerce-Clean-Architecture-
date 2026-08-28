namespace Ecommerce.Application.DTOs.ModelsRequest.Product
{
    public record CreateProductRequest(string Name, string? Description, string? ImageUrl, decimal Price, int Stock, int ParentCategoryId, int ChildCategoryId)
    {
    }
}
