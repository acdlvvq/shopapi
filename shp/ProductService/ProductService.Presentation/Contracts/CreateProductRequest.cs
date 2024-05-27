namespace ProductService.Presentation.Contracts;

public record CreateProductRequest(
    string Name, string Description, decimal Price, long AvailableAmount);
