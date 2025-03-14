using System.ComponentModel.DataAnnotations.Schema;

namespace SparkPlugSln.Domain.Entities;

public class ProductCategory : BaseEntity<int>
{
    public string Title { get; set; }
    public int? ParentId { get; set; }

    #region Relations

    [ForeignKey("ParentId")]
    public ProductCategory Parent { get; set; }

    #endregion
}