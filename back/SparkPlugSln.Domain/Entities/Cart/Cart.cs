using SparkPlugSln.Domain.Entities.User;
using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Entities.Cart;

public class Cart : BaseEntity<ulong>
{
    public CartStatus CartStatus { get; set; }
    public ulong? AddressId { get; set; }
    public ulong ShipmentFee { get; set; }
    public DateTime? DatePaid { get; set; }

    #region Relations

    public ICollection<CartItem> CartItems { get; set; }
    public UserAddress? Address { get; set; }

    #endregion
}