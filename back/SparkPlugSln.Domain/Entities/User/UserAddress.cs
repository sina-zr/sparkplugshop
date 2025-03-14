using System.ComponentModel.DataAnnotations.Schema;

namespace SparkPlugSln.Domain.Entities.User;

public class UserAddress: BaseEntity<ulong>
{
    public Guid UserId { get; set; }
    public string Address { get; set; }

    #region Relations

    [ForeignKey("UserId")]
    public User User { get; set; }

    #endregion
}