using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkPlugSln.Application.Helpers;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Enums;
using SparkPlugSln.Domain.Models.Cart;

namespace SparkPlugSln.Api.Controllers;

[ApiController]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;
    public CartController(ICartService cartService, IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
    }

    /// <summary>
    /// Save User Card
    /// </summary>
    /// <param name="cartDto"></param>
    /// <response code="200">successfully saved</response>
    /// <response code="400">invalid inputs</response>
    /// <response code="404">user not found</response>
    /// <response code="404">address not found</response>
    /// <response code="500">server error</response>
    /// <returns>just status codes</returns>
    [HttpPost("/[action]")]
    public async Task<IActionResult> SaveCart(CartDto cartDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("invalid inputs");
        }


        var user = await _userService.GetUserById(cartDto.UserId);
        var userId = User.GetId();
        if (user == null || cartDto.UserId != userId)
        {
            return NotFound("user not found");
        }

        if (cartDto.AddressId != null)
        {
            var address = await _userService.GetAddressById(cartDto.AddressId.Value);

            if (address == null)
            {
                return NotFound("address not found");
            }
        }

        var result = await _cartService.SaveCart(cartDto);

        if (!result)
        {
            return StatusCode(500, "server error");
        }

        return Ok("successfully saved");
    }

    /// <summary>
    /// Creates a payment session
    /// </summary>
    /// <param name="userId"></param>
    /// <response code="400">invalid input</response>
    /// <response code="404">user not found</response>
    /// <response code="500">server error</response>
    /// <returns>returns payment gateway url</returns>
    [HttpPost("/[action]")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePaymentSession([Required] Guid userId)
    {
        return Ok("https://zarinpal.com/paymentId");
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("/verify")]
    public async Task<IActionResult> VerifyPayment([FromQuery] string sessionId)
    {
        return Ok("https://your-react-app.com/payment-status");
    }


    #region Admin

    /// <summary>
    /// get list of orders for admin
    /// </summary>
    /// <param name="pageId"></param>
    /// <param name="pageSize"></param>
    /// <param name="filterUser"></param>
    /// <response code="403">forbidden</response>
    /// <response code="404">no order found</response>
    /// <response code="500">server error</response>
    /// <returns></returns>
    [HttpGet("/admin/[action]")]
    [ProducesResponseType(typeof(OrdersListVm), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetOrdersList(int pageId = 1, int pageSize = 100, string? filterUser = null)
    {
        var orders = await _cartService.GetOrdersList(pageId, pageSize, filterUser);

        if (orders == null)
        {
            return NotFound("no order found");
        }

        return Ok(orders);
    }

    /// <summary>
    /// get order's detail for admin
    /// </summary>
    /// <param name="cartId"></param>
    /// <response code="403">forbidden</response>
    /// <response code="404">order not found</response>
    /// <response code="500">server error</response>
    /// <returns></returns>
    [HttpGet("/admin/[action]")]
    [ProducesResponseType(typeof(OrderDetailsVm), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetOrderDetails(ulong cartId)
    {
        var order = await _cartService.GetOrderDetails(cartId);

        if (order == null)
        {
            return NotFound("no order found");
        }

        return Ok(order);
    }

    /// <summary>
    /// change order status
    /// </summary>
    /// <param name="cartId"></param>
    /// <param name="status"></param>
    /// <response code="200">successfully changed</response>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">order not found</response>
    /// <response code="500">server error</response>
    /// <returns></returns>
    [HttpPost("/admin/[action]")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ChangeOrderStatus([Required] ulong cartId, [Required] CartStatus status)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("invalid inputs");
        }

        var cart = await _cartService.GetCart(cartId);

        if (cart == null)
        {
            return NotFound("order not found");
        }


        var result = await _cartService.ChangeOrderStatus(cartId, status);

        if (!result)
        {
            return StatusCode(500, "server error");
        }

        return Ok("successfully changed");
    }

    /// <summary>
    /// Delete Order By Admin
    /// </summary>
    /// <param name="cartId"></param>
    /// <response code="403">forbidden</response>
    /// <response code="404">cart not found</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully deleted</returns>
    [HttpPost("/admin/[action]")]
    public async Task<IActionResult> DeleteOrder(ulong cartId)
    {
        var cart = await _cartService.GetCart(cartId);

        if (cart == null)
        {
            return NotFound("cart not found");
        }

        var result = await _cartService.DeleteOrder(cartId);

        if (!result)
        {
            return StatusCode(500, "server error");
        }

        return Ok("successfully deleted");
    }

    #endregion
}