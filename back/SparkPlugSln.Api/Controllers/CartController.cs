using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SparkPlugSln.Domain.Enums;
using SparkPlugSln.Domain.Models.Cart;

namespace SparkPlugSln.Api.Controllers;

[ApiController]
public class CartController : Controller
{
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
        // insert of update cart (don't duplicate)
        return Ok();
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
    public IActionResult GetOrdersList(int pageId = 1, int pageSize = 100, string? filterUser = null)
    {
        return Ok();
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
    public IActionResult GetOrderDetails(ulong cartId)
    {
        return Ok();
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
    public IActionResult ChangeOrderStatus(ulong cartId, CartStatus status)
    {
        return Ok();
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
    public IActionResult DeleteOrder(ulong cartId)
    {
        return Ok();
    }

    #endregion
}