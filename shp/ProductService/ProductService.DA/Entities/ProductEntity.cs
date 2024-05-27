namespace ProductService.DA.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long AvailableAmount { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
}
