using System.ComponentModel.DataAnnotations.Schema;

namespace SparkPlugSln.Domain.Entities.Cart;

public class CartItem:BaseEntity<ulong>
{
    public ulong CartId { get; set; }
    public ulong ProductId { get; set; }

    #region Relations

    [ForeignKey("CartId")]
    public Cart Cart { get; set; }

    #endregion
}