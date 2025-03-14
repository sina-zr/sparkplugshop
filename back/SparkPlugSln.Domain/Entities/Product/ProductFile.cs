using System.ComponentModel.DataAnnotations.Schema;
using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Entities;

public class ProductFile: BaseEntity<ulong>
{
    public ulong ProductId { get; set; }
    public string FileName { get; set; }
    public ProductFileTypes FileType { get; set; }

    #region Relations

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    #endregion
}