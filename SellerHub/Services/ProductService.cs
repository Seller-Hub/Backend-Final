using Microsoft.EntityFrameworkCore;
using SellerHub.Data;
using SellerHub.DTOs;
using SellerHub.Models;

namespace SellerHub.Services
{
    public class ProductService
    {
        private readonly AppDbContext _db;
        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<DashboardProductDto>> SearchSellerProductsAsync(int sellerId, string? search = null)
        {
            var query = _db.Products.AsQueryable().Where(p => p.SellerId == sellerId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) || p.ProductCode.Contains(search));

            var products = await query.ToListAsync();

            return products.Select(p => new DashboardProductDto(
                p.ProductId,
                p.ProductCode,
                p.Name,
                p.Price,
                p.Stock,
                p.TotalSold,
                p.Stock <= 0 ? "Out of stock" : p.Stock <= 10 ? "Low stock" : "In stock",
                p.Visibility
            )).ToList();
        }


        public async Task<List<DashboardProductDto>> FilterSellerProductsAsync(
        int sellerId,
        string? category = null,
        string? stockStatus = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
        {
            var query = _db.Products.AsQueryable().Where(p => p.SellerId == sellerId);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.ProductCategory != null && p.ProductCategory.Name == category);


            if (!string.IsNullOrWhiteSpace(stockStatus))
            {
                query = stockStatus.ToLower() switch
                {
                    "in stock" => query.Where(p => p.Stock > 10),
                    "low stock" => query.Where(p => p.Stock > 0 && p.Stock <= 10),
                    "out of stock" => query.Where(p => p.Stock <= 0),
                    _ => query
                };
            }

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var products = await query.OrderByDescending(p => p.ProductId).ToListAsync();

            return products.Select(p => new DashboardProductDto(
                p.ProductId,
                p.ProductCode,
                p.Name,
                p.Price,
                p.Stock,
                p.TotalSold,
                p.Stock <= 0 ? "Out of stock" : p.Stock <= 10 ? "Low stock" : "In stock",
                p.Visibility
            )).ToList();
        }

     
        public async Task<DashboardProductDto> CreateProductAsync(CreateProductDto dto, int sellerId)
        {
            var product = new Product
            {
                SellerId = sellerId,
                Name = dto.Name,

                Description = dto.Description,
                Size = dto.Size,
                Gender = dto.Gender,
                Colors = dto.Colors,
                DiscountType = dto.DiscountType,
                DiscountAmount = dto.DiscountAmount,
          
                Price = dto.Price,
                Stock = dto.Stock,
                ProductCode = dto.ProductCode,
                Visibility = dto.Visibility,
                PublishDate = dto.PublishDate
            };

            // (Many-to-Many)
            if (dto.CategoryIds != null && dto.CategoryIds.Any())
            {
                foreach (var catId in dto.CategoryIds)
                {
                    product.ProductProductCategories.Add(new ProductProductCategory
                    {
                        ProductCategoryId = catId
                    });
                }
            }

           

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

          
            return new DashboardProductDto(
                product.ProductId,
                product.ProductCode,
                product.Name,
                product.Price,
                product.Stock,
                product.TotalSold,
                product.StockStatus,
                product.Visibility
            );
        }

        public async Task<ProductCategoryDto?> CreateCategoryAsync(string name)
        {
            if (await _db.ProductCategories.AnyAsync(c => c.Name == name))
                return null; // does not add if it exists

            var category = new ProductCategory { Name = name };
            _db.ProductCategories.Add(category);
            await _db.SaveChangesAsync();

            return new ProductCategoryDto(category.Id, category.Name);
        }

        // Get all categories
        public async Task<List<ProductCategoryDto>> GetAllCategoriesAsync()
        {
            return await _db.ProductCategories
                .Select(c => new ProductCategoryDto(c.Id, c.Name))
                .ToListAsync();
        }


        public async Task<List<DashboardProductDto>> BulkEditProductsAsync(BulkEditProductDto dto, int sellerId)
        {
            var products = await _db.Products
                .Where(p => p.SellerId == sellerId && dto.ProductIds.Contains(p.ProductId))
                .ToListAsync();

            foreach (var product in products)
            {
                // Stock update
                if (dto.Stock.HasValue)
                    product.Stock = dto.Stock.Value;

                // Price update
                if (dto.Price.HasValue)
                    product.Price = dto.Price.Value;

                // Visibility update
                if (!string.IsNullOrWhiteSpace(dto.Visibility))
                    product.Visibility = dto.Visibility;

                // Categories update (Many-to-Many)
                if (dto.CategoryIds != null && dto.CategoryIds.Any())
                {
                    // Delete previous categories
                    var existingCategories = await _db.ProductProductCategories
                        .Where(pc => pc.ProductId == product.ProductId)
                        .ToListAsync();
                    _db.ProductProductCategories.RemoveRange(existingCategories);

                    // Add new categories
                    foreach (var catId in dto.CategoryIds)
                    {
                        _db.ProductProductCategories.Add(new ProductProductCategory
                        {
                            ProductId = product.ProductId,
                            ProductCategoryId = catId
                        });
                    }
                }
            }

            await _db.SaveChangesAsync();

            // Return updated products
            return products.Select(p => new DashboardProductDto(
                p.ProductId,
                p.ProductCode,
                p.Name,
                p.Price,
                p.Stock,
                p.TotalSold,
                p.Stock <= 0 ? "Out of stock" : p.Stock <= 10 ? "Low stock" : "In stock",
                p.Visibility
            )).ToList();
        }
    }
}