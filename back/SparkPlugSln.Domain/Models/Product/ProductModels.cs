using Microsoft.AspNetCore.Http;

namespace SparkPlugSln.Domain.Models.Product;

public class ProductVm
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public ulong Price { get; set; }
    public string? Description { get; set; }
}

public class ProductListVm
{
    public List<ProductVm>? Products { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public ulong? Price { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public Guid OwnerId { get; set; }
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public List<IFormFile>? Images { get; set; }
}

public class EditProductVm
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ulong? Price { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public Guid OwnerId { get; set; }
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public List<string>? Images { get; set; }
}

public class EditProductDto
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ulong? Price { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public Guid OwnerId { get; set; }
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public List<IFormFile>? Images { get; set; }
}