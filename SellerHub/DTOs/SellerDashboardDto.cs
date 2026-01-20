namespace SellerHub.DTOs
{
    public record SellerDashboardDto(
        decimal TotalSales,
        int TotalOrders,
        int StoreSessions,
        decimal OverallSales,
        decimal RegionalSales,
        OrdersOverviewDto OrdersOverview,
        List<TopSellingProductDto> TopSellingProducts
    );

    public record OrdersOverviewDto(int Completed, int Cancelled, int Ongoing);
    public record TopSellingProductDto(string ProductName, int QuantitySold);

}
