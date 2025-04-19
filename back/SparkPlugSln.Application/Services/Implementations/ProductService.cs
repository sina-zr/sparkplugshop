using Microsoft.EntityFrameworkCore;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Entities;
using SparkPlugSln.Domain.Enums;
using SparkPlugSln.Domain.IRepositories;
using SparkPlugSln.Domain.Models.Product;

namespace SparkPlugSln.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductListVm> GetProductsList(int pageId, int pageSize, string? filterName)
        {
            var query = _productRepository.GetAllProducts()
                .Include(p => p.ProductFiles)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterName))
            {
                query = query.Where(p => p.Name.Contains(filterName));
            }

            long totalCount = await query.LongCountAsync();
            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            pageId = Math.Clamp(pageId, 1, pageCount);
            int skip = (pageId - 1) * pageSize;

            var products = await query
                .OrderByDescending(p => p.CreateDate)
                .Skip(skip)
                .Take(pageSize)
                .Select(p => new ProductVm
                {
                    Id = (int)p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price ?? 0,
                    ImageUrl = p.ProductFiles.Where(f => f.FileType == ProductFileTypes.Image).Select(f => f.FileName).FirstOrDefault(),
                }).ToListAsync();

            return new ProductListVm
            {
                Products = products,
                CurrentPage = pageId,
                TotalPages = pageCount
            };
        }

        public async Task<bool> CreateProduct(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                DiscountPercentage = createProductDto.DiscountPercentage,
                OwnerId = createProductDto.OwnerId,
                CategoryId = createProductDto.CategoryId,
                IsAvailable = createProductDto.IsAvailable,
                CreateDate = DateTime.Now
            };

            await _productRepository.AddProduct(product);

            return true;
        }

        public async Task<EditProductVm> GetProductForEdit(ulong productId)
        {
            var product = await _productRepository.GetAllProducts()
                .Include(p => p.ProductFiles)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return null;

            return new EditProductVm
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPercentage = product.DiscountPercentage,
                OwnerId = product.OwnerId,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
                Images = product.ProductFiles
                    .Where(f => f.FileType == ProductFileTypes.Image)
                    .Select(f => f.FileName)
                    .ToList()
            };
        }

        public async Task<bool> EditProduct(EditProductDto editProductDto)
        {
            var product = await _productRepository.GetAllProducts()
                .Include(p => p.ProductFiles)
                .FirstOrDefaultAsync(p => p.Id == editProductDto.Id);

            if (product == null)
                return false;

            product.Name = editProductDto.Name;
            product.Description = editProductDto.Description;
            product.Price = editProductDto.Price;
            product.DiscountPercentage = editProductDto.DiscountPercentage;
            product.CategoryId = editProductDto.CategoryId;
            product.IsAvailable = editProductDto.IsAvailable;

            _productRepository.UpdateProduct(product);
            return true;
        }

        public async Task<bool> DeleteProduct(ulong productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null)
                return false;

            _productRepository.DeleteProduct(product);
            return true;
        }

        public async Task<ProductVm> GetProductDetails(ulong productId)
        {
            var product = await _productRepository.GetAllProducts()
                .Include(p => p.ProductFiles)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return null;

            return new ProductVm
            {
                Id = (int)product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price ?? 0,
                ImageUrl = product.ProductFiles
                    .FirstOrDefault(f => f.FileType == ProductFileTypes.Image)?.FileName,
            };
        }
    }
}