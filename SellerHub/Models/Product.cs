using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SellerHub.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Size { get; set; } // S, M, L, XL, etc..

        [MaxLength(20)]
        public string? Gender { get; set; } // Male, Female

        [MaxLength(100)]
        public string? Colors { get; set; } 


        [Required]
        public int Stock { get; set; } // Stock Amount

        // 👉 YENİ EKLENEN: İndirim Bilgileri
        [MaxLength(50)]
        public string? DiscountType { get; set; } // fixed, percentage

        public decimal? DiscountAmount { get; set; }


        public int TotalSold { get; set; } = 0;  // Total Quantity

        [MaxLength(50)]
        public string ProductCode { get; set; } = string.Empty; // code of products, for example: 03-495837521

        [MaxLength(20)]
        public string Visibility { get; set; } = "draft"; // draft, published, hidden


        public int? ProductCategoryId { get; set; }

        public DateTime? PublishDate { get; set; } // datetime?
        public ProductCategory? ProductCategory { get; set; }

        // Navigation property
        public User Seller { get; set; } = null!;

        // Computed property (optional)
        [NotMapped]
        public string StockStatus
        {
            get
            {
                if (Stock <= 0) return "Out of stock";
                if (Stock <= 10) return "Low stock";
                return "In stock";
            }
        }


        public ICollection<ProductProductCategory> ProductProductCategories { get; set; } = new List<ProductProductCategory>();
    }
}
