using SparkPlugSln.Domain.Entities.Cart;

namespace SparkPlugSln.Domain.IRepositories;

public interface ICartRepository
{
    #region Cart

    IQueryable<Cart> GetAllCarts();

    Task<Cart?> GetCartById(int id);

    Task<Cart> AddCart(Cart cart);

    Task<IEnumerable<Cart>> AddRangeCarts(IEnumerable<Cart> carts);

    Cart UpdateCart(Cart cart);

    IEnumerable<Cart> UpdateRangeCarts(IEnumerable<Cart> carts);

    bool DeleteCart(Cart cart);

    bool DeleteRangeCarts(IEnumerable<Cart> carts);

    #endregion

    #region CartItem

    IQueryable<CartItem> GetAllCartItems();

    Task<CartItem?> GetCartItemById(int id);

    Task<CartItem> AddCartItem(CartItem cartItem);

    Task<IEnumerable<CartItem>> AddRangeCartItems(IEnumerable<CartItem> cartItems);

    CartItem UpdateCartItem(CartItem cartItem);

    IEnumerable<CartItem> UpdateRangeCartItems(IEnumerable<CartItem> cartItems);

    bool DeleteCartItem(CartItem cartItem);

    bool DeleteRangeCartItems(IEnumerable<CartItem> cartItems);

    #endregion
}