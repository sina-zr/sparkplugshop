using Microsoft.AspNetCore.Mvc;
using SparkPlugSln.Domain.Models.Product;

namespace SparkPlugSln.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : Controller
{
    public ProductController()
    {
        
    }

    /// <summary>
    /// GetProductsList for users
    /// </summary>
    /// <param name="pageId">default is 1</param>
    /// <param name="pageSize">default is 9</param>
    /// <param name="filterName">default is empty</param>
    /// <response code="404">no item found</response>
    /// <returns>list of products with pagination</returns>
    [HttpGet("/products")]
    [ProducesResponseType(typeof(ProductListVm), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsList(int pageId = 1, int pageSize = 9, string? filterName = "")
    {
        return Ok();
    }


    #region Admin

    /// <summary>
    /// Get ProductsList For Admin
    /// </summary>
    /// <param name="pageId">default is 1</param>
    /// <param name="pageSize">default is 100</param>
    /// <param name="filterName">default is empty</param>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">no item found</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully created</returns>
    [HttpPost("/admin/GetProductsList")]
    [ProducesResponseType(typeof(ProductListVm), StatusCodes.Status200OK)]
    public IActionResult GetProductsListForAdmin(int pageId = 1, int pageSize = 100, string? filterName = "")
    {
        return Ok();
    }
    
    /// <summary>
    /// Create Product By Admin
    /// </summary>
    /// <param name="createProductDto"></param>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully created</returns>
    [HttpPost("/admin/[action]")]
    public IActionResult CreateProduct(CreateProductDto createProductDto)
    {
        return Ok();
    }

    /// <summary>
    /// Get Product for edit By Admin
    /// </summary>
    /// <param name="productId"></param>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">product not found</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully edited</returns>
    [HttpPost("/admin/[action]")]
    [ProducesResponseType(typeof(EditProductVm), StatusCodes.Status200OK)]
    public IActionResult GetProductForEdit(ulong productId)
    {
        return Ok();
    }
    
    /// <summary>
    /// Edit Product By Admin
    /// </summary>
    /// <param name="editProductDto"></param>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">product not found</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully edited</returns>
    [HttpPost("/admin/[action]")]
    public IActionResult EditProduct(EditProductDto editProductDto)
    {
        return Ok();
    }

    /// <summary>
    /// Delete Product By Admin
    /// </summary>
    /// <param name="productId"></param>
    /// <response code="403">forbidden</response>
    /// <response code="404">product not found</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully deleted</returns>
    [HttpPost("/admin/[action]")]
    public IActionResult DeleteProduct(ulong productId)
    {
        return Ok();
    }

    #endregion
}