namespace ProductService.Core.Models;

public class Product
{
    public Product(
        Guid id,
        string name,
        string description,
        decimal price,
        long availableAmount,
        Guid creatorId,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        AvailableAmount = availableAmount;
        CreatorId = creatorId;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public long AvailableAmount { get; private set; }
    public Guid CreatorId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Product Create(
        string name, string description, decimal price, long available, Guid creator)
    {
        return new Product(
            Guid.NewGuid(),
            name,
            description,
            price,
            available,
            creator,
            DateTime.UtcNow
        );  
    }

    public Product SetName(string name)
    {
        Name = name; 
        return this;
    }

    public Product SetDescription(string description) 
    { 
        Description = description; 
        return this;
    }

    public Product SetPrice(decimal price) 
    {
        Price = price;
        return this;
    }

    public Product SetAvailableAmount(long availableAmount)
    {
        AvailableAmount = availableAmount;
        return this;
    }
}
