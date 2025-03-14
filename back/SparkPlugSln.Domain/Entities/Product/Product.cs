using System.ComponentModel.DataAnnotations.Schema;

namespace SparkPlugSln.Domain.Entities;

public class Product : BaseEntity<ulong>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public ulong? Price { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public Guid OwnerId { get; set; }
    public int? CategoryId { get; set; }
    public bool IsAvailable { get; set; }

    #region Relations

    [ForeignKey("CategoryId")]
    public ProductCategory Category { get; set; }
    
    public ICollection<ProductComment> ProductComments { get; set; }

    [ForeignKey("OwnerId")]
    public User.User Owner { get; set; }

    public ICollection<ProductFile> ProductFiles { get; set; }

    #endregion
}