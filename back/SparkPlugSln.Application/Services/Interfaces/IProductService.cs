using SparkPlugSln.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkPlugSln.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductListVm> GetProductsList(int pageId, int pageSize, string? filterName);
        Task<bool> CreateProduct(CreateProductDto createProductDto);
        Task<EditProductVm> GetProductForEdit(ulong productId);
        Task<bool> EditProduct(EditProductDto editProductDto);
        Task<bool> DeleteProduct(ulong productId);
    }
}
