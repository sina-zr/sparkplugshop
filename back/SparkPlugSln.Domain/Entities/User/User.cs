using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Entities.User;

public class User : BaseEntity<Guid>
{
    public string FullName { get; set; } = "";
    public string Tell { get; set; }
    public string? Password { get; set; }
    public string VerificationCode { get; set; }
    public bool IsTellVerified { get; set; } = false;
    public UserRoles Role { get; set; } = UserRoles.User;
    public string? ProfileImageName { get; set; }

    #region Relations

    public ICollection<UserAddress> UserAddresses { get; set; }
    public ICollection<Cart.Cart> Carts { get; set; }

    #endregion
}