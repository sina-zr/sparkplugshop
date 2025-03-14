using SparkPlugSln.Domain.Enums;

namespace SparkPlugSln.Domain.Models.Cart;

public class OrderVm
{
    public ulong CartId { get; set; }
    public string FullName { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? DatePaid { get; set; }
    public ulong FinalPrice { get; set; }
    public CartStatus CartStatus { get; set; }
}

public class OrdersListVm
{
    public List<OrderVm> Orders { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class OrderDetailsVm
{
    public ulong CartId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? DatePaid { get; set; }
    public ulong FinalPrice { get; set; }
    public CartStatus CartStatus { get; set; }
    public string Address { get; set; }
}