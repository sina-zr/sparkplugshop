using System.ComponentModel.DataAnnotations;

namespace SparkPlugSln.Domain.Models.User;

public class AddressVm
{
    public ulong Id { get; set; }
    public string Address { get; set; }
}

public class AddAddressDto
{
    [Required] public string Address { get; set; }
    [Required] public Guid UserId { get; set; }
}