using SparkPlugSln.Domain.Entities;
using SparkPlugSln.Domain.IRepositories;

namespace SparkPlugSln.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PersistenceDbContext _context;

    public ProductRepository(PersistenceDbContext context)
    {
        _context = context;
    }

    #region Product

    public IQueryable<Product> GetAllProducts()
    {
        return _context.Products.AsQueryable();
    }

    public async Task<Product?> GetProductById(ulong id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> AddProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<IEnumerable<Product>> AddRangeProducts(IEnumerable<Product> products)
    {
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
        return products;
    }

    public Product UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
        return product;
    }

    public IEnumerable<Product> UpdateRangeProducts(IEnumerable<Product> products)
    {
        _context.Products.UpdateRange(products);
        _context.SaveChanges();
        return products;
    }

    public bool DeleteProduct(Product product)
    {
        try
        {
            _context.Products.Remove(product);
            var changes = _context.SaveChanges();
            if (changes > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            // Log
            return false;
        }
    }

    public bool DeleteRangeProducts(IEnumerable<Product> products)
    {
        try
        {
            _context.Products.RemoveRange(products);
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

    #region ProductCategory

    public IQueryable<ProductCategory> GetAllProductCategories()
    {
        return _context.ProductCategories.AsQueryable();
    }

    public async Task<ProductCategory?> GetProductCategoryById(int id)
    {
        return await _context.ProductCategories.FindAsync(id);
    }

    public async Task<ProductCategory> AddProductCategory(ProductCategory productCategory)
    {
        await _context.ProductCategories.AddAsync(productCategory);
        await _context.SaveChangesAsync();
        return productCategory;
    }

    public async Task<IEnumerable<ProductCategory>> AddRangeProductCategories(
        IEnumerable<ProductCategory> productCategories)
    {
        await _context.ProductCategories.AddRangeAsync(productCategories);
        await _context.SaveChangesAsync();
        return productCategories;
    }

    public ProductCategory UpdateProductCategory(ProductCategory productCategory)
    {
        _context.ProductCategories.Update(productCategory);
        _context.SaveChanges();
        return productCategory;
    }

    public IEnumerable<ProductCategory> UpdateRangeProductCategories(IEnumerable<ProductCategory> productCategories)
    {
        _context.ProductCategories.UpdateRange(productCategories);
        _context.SaveChanges();
        return productCategories;
    }

    public bool DeleteProductCategory(ProductCategory productCategory)
    {
        try
        {
            _context.ProductCategories.Remove(productCategory);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    public bool DeleteRangeProductCategories(IEnumerable<ProductCategory> productCategories)
    {
        try
        {
            _context.ProductCategories.RemoveRange(productCategories);
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

    #region Comment

    public IQueryable<ProductComment> GetAllComments()
    {
        return _context.ProductsComments.AsQueryable();
    }

    public async Task<ProductComment?> GetCommentById(ulong id)
    {
        return await _context.ProductsComments.FindAsync(id);
    }

    public async Task<ProductComment> AddComment(ProductComment comment)
    {
        await _context.ProductsComments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<IEnumerable<ProductComment>> AddRangeComments(IEnumerable<ProductComment> comments)
    {
        await _context.ProductsComments.AddRangeAsync(comments);
        await _context.SaveChangesAsync();
        return comments;
    }

    public ProductComment UpdateComment(ProductComment comment)
    {
        _context.ProductsComments.Update(comment);
        _context.SaveChanges();
        return comment;
    }

    public IEnumerable<ProductComment> UpdateRangeComments(IEnumerable<ProductComment> comments)
    {
        _context.ProductsComments.UpdateRange(comments);
        _context.SaveChanges();
        return comments;
    }

    public bool DeleteComment(ProductComment comment)
    {
        try
        {
            _context.ProductsComments.Remove(comment);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        catch (Exception e)
        {
            // Log exception
            return false;
        }
    }

    public bool DeleteRangeComments(IEnumerable<ProductComment> comments)
    {
        try
        {
            _context.ProductsComments.RemoveRange(comments);
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

    #region ProductsFiles

    public IQueryable<ProductFile> GetAllProductImages()
    {
        return _context.ProductsFiles.AsQueryable();
    }

    public async Task<IEnumerable<ProductFile>> AddRangeProductImages(IEnumerable<ProductFile> productImages)
    {
        await _context.ProductsFiles.AddRangeAsync(productImages);
        await _context.SaveChangesAsync();
        return productImages;
    }

    public bool DeleteRangeProductImages(IEnumerable<ProductFile> images)
    {
        try
        {
            _context.ProductsFiles.RemoveRange(images);
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