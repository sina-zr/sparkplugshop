using SparkPlugSln.Domain.Entities.Cart;
using SparkPlugSln.Domain.Enums;
using SparkPlugSln.Domain.Models.Cart;

namespace SparkPlugSln.Application.Services.Interfaces;

public interface ICartService
{
    Task<OrdersListVm> GetOrdersList(int pageId, int pageSize, string? filterUser);
    Task<OrderDetailsVm> GetOrderDetails(ulong cartId);
    Task<Cart> GetCart(ulong cartId);
    Task<bool> ChangeOrderStatus(ulong cartId, CartStatus status);
    Task<bool> DeleteOrder(ulong cartId);
    Task<bool> SaveCart(CartDto cartDto);
}