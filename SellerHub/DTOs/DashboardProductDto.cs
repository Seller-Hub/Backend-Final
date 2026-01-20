namespace SellerHub.DTOs
{
    public record DashboardProductDto(
        int ProductId,
        string ProductCode,
        string Name,
        decimal Price,
        int Stock,
        int TotalSold,
        string StockStatus,
        string Visibility
    );

    public record CreateProductDto(
         string Name,
         string? Description, 
         string? Size,        
         string? Gender,     
         string? Colors,      
         decimal Price,
         int Stock,
         string? DiscountType,   
         decimal? DiscountAmount,
         string ProductCode,
         string Visibility,
         DateTime? PublishDate,
         List<int>? CategoryIds = null,
         List<string>? Tags = null 
     );

    public record CreateProductCategoryDto(string Name);
    public record ProductCategoryDto(int Id, string Name);

    public record BulkEditProductDto(
    List<int> ProductIds,
    List<int>? CategoryIds = null,
    int? Stock = null,
    decimal? Price = null,
    string? Visibility = null
);


}
