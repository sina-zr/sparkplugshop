using System.ComponentModel.DataAnnotations.Schema;

namespace SparkPlugSln.Domain.Entities;

public class ProductComment : BaseEntity<ulong>
{
    public string Text { get; set; }
    public ulong ProductId { get; set; }
    public byte? Rate { get; set; }

    #region Relations

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    #endregion
}