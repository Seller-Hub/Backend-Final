namespace SellerHub.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int SellerId { get; set; }  // UserId of seller
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "ongoing"; // completed, cancelled, ongoing
        public string Region { get; set; } = string.Empty;

        public User Seller { get; set; } = null!;
    }
}
