using System.ComponentModel.DataAnnotations;

namespace SparkPlugSln.Domain.Models.Cart;

public class CartItemDto
{
    public ulong Product { get; set; }
    public int Count { get; set; }
}

public class CartDto
{
    public Guid UserId { get; set; }
    [Required] public List<CartItemDto> Items { get; set; }
    public ulong? AddressId { get; set; }
}