using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Entities;
using SparkPlugSln.Domain.IRepositories;
using SparkPlugSln.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkPlugSln.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> CreateProduct(CreateProductDto createProductDto)
        {
            var product = new Product();
            product.Name = createProductDto.Name;
            product.Description = createProductDto.Description;
            product.Price = createProductDto.Price;
            product.DiscountPercentage = createProductDto.DiscountPercentage;
            product.OwnerId = createProductDto.OwnerId;
            product.CategoryId = createProductDto.CategoryId;
            product.IsAvailable = createProductDto.IsAvailable;

            await _productRepository.AddProduct(product);

            return true;
        }

        public Task<bool> DeleteProduct(ulong productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditProduct(EditProductDto editProductDto)
        {
            throw new NotImplementedException();
        }

        public Task<EditProductVm> GetProductForEdit(ulong productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductListVm> GetProductsList(int pageId, int pageSize, string? filterName)
        {
            throw new NotImplementedException();
        }
    }
}
