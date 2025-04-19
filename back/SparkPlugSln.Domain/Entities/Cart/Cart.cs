using System.ComponentModel.DataAnnotations.Schema;
using SparkPlugSln.Domain.Entities.User;
using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Entities.Cart;

public class Cart : BaseEntity<ulong>
{
    public Guid UserId { get; set; }
    public CartStatus CartStatus { get; set; }
    public ulong? AddressId { get; set; }
    public ulong ShipmentFee { get; set; }
    public DateTime? DatePaid { get; set; }

    #region Relations

    [ForeignKey("UserId")]
    public User.User User { get; set; }

    public ICollection<CartItem> CartItems { get; set; }
    public UserAddress? Address { get; set; }

    #endregion
}