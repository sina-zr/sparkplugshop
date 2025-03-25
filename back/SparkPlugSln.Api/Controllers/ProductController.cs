using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Models.Product;

namespace SparkPlugSln.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// GetProductsList for users
    /// </summary>
    /// <param name="pageId">default is 1</param>
    /// <param name="pageSize">default is 9</param>
    /// <param name="filterName">default is empty</param>
    /// <response code="200">Returns list of products</response>
    /// <response code="404">no item found</response>
    /// <returns>list of products with pagination</returns>
    [HttpGet("/products")]
    [ProducesResponseType(typeof(ProductListVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsList(int pageId = 1, int pageSize = 9, string? filterName = "")
    {
        var result = await _productService.GetProductsList(pageId, pageSize, filterName);

        if (result == null || result.Products == null || !result.Products.Any())
        {
            return NotFound("No products found");
        }

        return Ok(result);
    }

    #region Admin Endpoints

    /// <summary>
    /// Get ProductsList For Admin
    /// </summary>
    /// <param name="pageId">default is 1</param>
    /// <param name="pageSize">default is 100</param>
    /// <param name="filterName">default is empty</param>
    /// <response code="200">Returns list of products</response>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">no item found</response>
    /// <returns>List of products with pagination</returns>
    [HttpGet("/admin/products")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ProductListVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsListForAdmin(int pageId = 1, int pageSize = 100, string? filterName = "")
    {
        if (pageId < 1 || pageSize < 1)
        {
            return BadRequest("Invalid pagination parameters");
        }

        var result = await _productService.GetProductsList(pageId, pageSize, filterName);

        if (result == null || result.Products == null || !result.Products.Any())
        {
            return NotFound("No products found");
        }

        return Ok(result);
    }

    /// <summary>
    /// Create Product By Admin
    /// </summary>
    /// <param name="createProductDto">Product data</param>
    /// <response code="200">Product created successfully</response>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="500">server error</response>
    /// <returns>Creation result</returns>
    [HttpPost("/admin/products")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _productService.CreateProduct(createProductDto);
            return result ? Ok(new { Message = "Product created successfully" })
                         : StatusCode(500, "Failed to create product");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Get Product for edit By Admin
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <response code="200">Returns product details</response>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">product not found</response>
    /// <returns>Product details for editing</returns>
    [HttpGet("/admin/products/{productId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(EditProductVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductForEdit(ulong productId)
    {
        if (productId == 0)
        {
            return BadRequest("Invalid product ID");
        }

        var product = await _productService.GetProductForEdit(productId);

        return product == null ? NotFound("Product not found") : Ok(product);
    }

    /// <summary>
    /// Edit Product By Admin
    /// </summary>
    /// <param name="editProductDto">Product data</param>
    /// <response code="200">Product updated successfully</response>
    /// <response code="400">invalid inputs</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">product not found</response>
    /// <response code="500">server error</response>
    /// <returns>Update result</returns>
    [HttpPut("/admin/products")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditProduct([FromBody] EditProductDto editProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _productService.EditProduct(editProductDto);

            if (!result)
            {
                return NotFound("Product not found");
            }

            return Ok(new { Message = "Product updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete Product By Admin
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <response code="200">Product deleted successfully</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">product not found</response>
    /// <response code="500">server error</response>
    /// <returns>Deletion result</returns>
    [HttpDelete("/admin/products/{productId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProduct(ulong productId)
    {
        try
        {
            var result = await _productService.DeleteProduct(productId);

            if (!result)
            {
                return NotFound("Product not found");
            }

            return Ok(new { Message = "Product deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    #endregion
}