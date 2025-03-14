using SparkPlugSln.Domain.Entities.Cart;
using SparkPlugSln.Domain.IRepositories;

namespace SparkPlugSln.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly PersistenceDbContext _context;

    public CartRepository(PersistenceDbContext context)
    {
        _context = context;
    }

    public IQueryable<Cart> GetAllCarts()
    {
        return _context.Carts.AsQueryable();
    }

    public async Task<Cart?> GetCartById(int id)
    {
        return await _context.Carts.FindAsync(id);
    }

    public async Task<Cart> AddCart(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<IEnumerable<Cart>> AddRangeCarts(IEnumerable<Cart> carts)
    {
        await _context.Carts.AddRangeAsync(carts);
        await _context.SaveChangesAsync();
        return carts;
    }

    public Cart UpdateCart(Cart cart)
    {
        _context.Carts.Update(cart);
        _context.SaveChanges();
        return cart;
    }

    public IEnumerable<Cart> UpdateRangeCarts(IEnumerable<Cart> carts)
    {
        _context.Carts.UpdateRange(carts);
        _context.SaveChanges();
        return carts;
    }

    public bool DeleteCart(Cart cart)
    {
        try
        {
            _context.Carts.Remove(cart);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    public bool DeleteRangeCarts(IEnumerable<Cart> carts)
    {
        try
        {
            _context.Carts.RemoveRange(carts);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    #region CartItem

    public IQueryable<CartItem> GetAllCartItems()
    {
        return _context.CartItems.AsQueryable();
    }

    public async Task<CartItem?> GetCartItemById(int id)
    {
        return await _context.CartItems.FindAsync(id);
    }

    public async Task<CartItem> AddCartItem(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
        return cartItem;
    }

    public async Task<IEnumerable<CartItem>> AddRangeCartItems(IEnumerable<CartItem> cartItems)
    {
        await _context.CartItems.AddRangeAsync(cartItems);
        await _context.SaveChangesAsync();
        return cartItems;
    }

    public CartItem UpdateCartItem(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        _context.SaveChanges();
        return cartItem;
    }

    public IEnumerable<CartItem> UpdateRangeCartItems(IEnumerable<CartItem> cartItems)
    {
        _context.CartItems.UpdateRange(cartItems);
        _context.SaveChanges();
        return cartItems;
    }

    public bool DeleteCartItem(CartItem cartItem)
    {
        try
        {
            _context.CartItems.Remove(cartItem);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    public bool DeleteRangeCartItems(IEnumerable<CartItem> cartItems)
    {
        try
        {
            _context.CartItems.RemoveRange(cartItems);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    #endregion
}