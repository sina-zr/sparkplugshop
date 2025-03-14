using SparkPlugSln.Domain.Entities;

namespace SparkPlugSln.Domain.IRepositories;

public interface IProductRepository
{
    #region Product

    IQueryable<Product> GetAllProducts();
    Task<Product?> GetProductById(ulong id);
    Task<Product> AddProduct(Product product);
    Task<IEnumerable<Product>> AddRangeProducts(IEnumerable<Product> products);
    Product UpdateProduct(Product product);
    IEnumerable<Product> UpdateRangeProducts(IEnumerable<Product> products);
    bool DeleteProduct(Product product);
    bool DeleteRangeProducts(IEnumerable<Product> products);

    #endregion

    #region ProductCategory

    IQueryable<ProductCategory> GetAllProductCategories();

    Task<ProductCategory?> GetProductCategoryById(int id);

    Task<ProductCategory> AddProductCategory(ProductCategory productCategory);

    Task<IEnumerable<ProductCategory>> AddRangeProductCategories(
        IEnumerable<ProductCategory> productCategories);

    ProductCategory UpdateProductCategory(ProductCategory productCategory);

    IEnumerable<ProductCategory> UpdateRangeProductCategories(IEnumerable<ProductCategory> productCategories);

    bool DeleteProductCategory(ProductCategory productCategory);

    bool DeleteRangeProductCategories(IEnumerable<ProductCategory> productCategories);

    #endregion

    #region Comment

    IQueryable<ProductComment> GetAllComments();
    Task<ProductComment?> GetCommentById(ulong id);
    Task<ProductComment> AddComment(ProductComment comment);
    Task<IEnumerable<ProductComment>> AddRangeComments(IEnumerable<ProductComment> comments);
    ProductComment UpdateComment(ProductComment comment);
    IEnumerable<ProductComment> UpdateRangeComments(IEnumerable<ProductComment> comments);
    bool DeleteComment(ProductComment comment);
    bool DeleteRangeComments(IEnumerable<ProductComment> comments);

    #endregion

    #region ProductsFiles

    IQueryable<ProductFile> GetAllProductImages();
    Task<IEnumerable<ProductFile>> AddRangeProductImages(IEnumerable<ProductFile> productImages);
    bool DeleteRangeProductImages(IEnumerable<ProductFile> images);

    #endregion
}