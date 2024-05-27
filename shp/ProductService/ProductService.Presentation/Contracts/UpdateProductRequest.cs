namespace ProductService.Presentation.Contracts;

public record UpdateProductRequest(
    string Name, string Description, decimal Price, long AvailableAmount);
