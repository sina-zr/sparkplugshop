using System.ComponentModel.DataAnnotations.Schema;

namespace SparkPlugSln.Domain.Entities.Cart;

public class CartItem : BaseEntity<ulong>
{
    public ulong CartId { get; set; }
    public ulong ProductId { get; set; }
    public int Count { get; set; }

    #region Relations

    [ForeignKey("CartId")]
    public Cart Cart { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    #endregion
}