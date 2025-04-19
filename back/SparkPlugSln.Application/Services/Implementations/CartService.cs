using Microsoft.EntityFrameworkCore;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Entities.Cart;
using SparkPlugSln.Domain.Enums;
using SparkPlugSln.Domain.IRepositories;
using SparkPlugSln.Domain.Models.Cart;

namespace SparkPlugSln.Application.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<bool> ChangeOrderStatus(ulong cartId, CartStatus status)
    {
        var cart = await _cartRepository.GetCartById(cartId);
        if (cart == null)
        {
            return false;
        }

        cart.CartStatus = status;
        _cartRepository.UpdateCart(cart);
        return true;
    }

    public async Task<bool> DeleteOrder(ulong cartId)
    {
        var cart = await _cartRepository.GetCartById(cartId);
        if (cart == null)
        {
            return false;
        }

        _cartRepository.DeleteCart(cart);
        return true;
    }

    public async Task<Cart?> GetCart(ulong cartId)
    {
        return await _cartRepository.GetCartById(cartId);
    }

    public async Task<OrderDetailsVm> GetOrderDetails(ulong cartId)
    {
        var cart = await _cartRepository.GetAllCarts()
            .Include(c => c.User)
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Product)
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
        {
            return null;
        }

        return new OrderDetailsVm()
        {
            CartId = cart.Id,
            FullName = cart.User.FullName,
            CreateDate = cart.CreateDate,
            DatePaid = cart.DatePaid,
            FinalPrice = (ulong)cart.CartItems.Sum(i => (decimal)i.Product.Price),
            CartStatus = cart.CartStatus,
            Address = cart.Address.Address
        };
    }

    public async Task<OrdersListVm> GetOrdersList(int pageId, int pageSize, string? filterUser)
    {
        var query = _cartRepository.GetAllCarts()
            .Include(c => c.User)
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Product)
            .Where(c => !c.IsDelete);

        if (!string.IsNullOrEmpty(filterUser))
        {
            query = query.Where(c => c.User.FullName.Contains(filterUser));
        }


        // pagination
        int totalCount = await query.CountAsync();
        int pageCount = (int)(Math.Ceiling((decimal)totalCount / pageSize));
        if (pageId > pageCount)
        {
            pageId = pageCount;
        }

        int skip = (pageId - 1) * pageSize;
        if (skip < 0)
        {
            skip = 0;
        }

        var orders = await query.OrderByDescending(c => c.CreateDate).Skip(skip).Take(pageSize)
            .Select(c => new OrderVm()
            {
                CartId = c.Id,
                FullName = c.User.FullName,
                CreateDate = c.CreateDate,
                DatePaid = c.DatePaid,
                FinalPrice = (ulong)c.CartItems.Sum(i => (decimal)i.Product.Price),
                CartStatus = c.CartStatus
            }).ToListAsync();

        return new OrdersListVm()
        {
            Orders = orders,
            CurrentPage = pageId,
            TotalPages = pageCount
        };
    }

    public async Task<bool> SaveCart(CartDto cartDto)
    {
        var cart = await _cartRepository.GetAllCarts()
            .FirstOrDefaultAsync(c => c.UserId == cartDto.UserId);

        if (cart == null)
        {
            cart = new Cart();

            cart.AddressId = cartDto.AddressId;
            cart.CartStatus = CartStatus.NotPaid;
            cart.UserId = cartDto.UserId;
            cart.ShipmentFee = cartDto.ShipmentFee;

            await _cartRepository.AddCart(cart);

            cart.UserId = cartDto.UserId;
            var items = cartDto.Items.Select(i => new CartItem()
            {
                ProductId = i.Product,
                Count = i.Count,
                CartId = cart.Id
            }).ToList();

            await _cartRepository.AddRangeCartItems(items);
        }
        else
        {
            // Update cart properties
            cart.AddressId = cartDto.AddressId;
            cart.ShipmentFee = cartDto.ShipmentFee;
            _cartRepository.UpdateCart(cart);

            // Get current cart items
            var currentItems = await _cartRepository.GetAllCartItems()
                .Where(ci => ci.CartId == cart.Id)
                .ToListAsync();

            // Delete items that are not in the new list
            var itemsToDelete = currentItems
                .Where(ci => !cartDto.Items.Any(i => i.Product == ci.ProductId))
                .ToList();

            if (itemsToDelete.Any())
            {
                _cartRepository.DeleteRangeCartItems(itemsToDelete);
            }

            // add new items
            var itemsToAdd = cartDto.Items.Where(i => !currentItems.Any(ci => ci.ProductId == i.Product))
                .Select(i => new CartItem()
                {
                    ProductId = i.Product,
                    Count = i.Count,
                    CartId = cart.Id
                }).ToList();

            if (itemsToAdd.Any())
            {
                await _cartRepository.AddRangeCartItems(itemsToAdd);
            }

            // Update existing items
            var existingItems = currentItems.Where(ci => cartDto.Items.Any(i => i.Product == ci.ProductId)).ToList();
            if (existingItems.Any())
            {
                _cartRepository.UpdateRangeCartItems(existingItems);
            }
        }

        return true;
    }
}