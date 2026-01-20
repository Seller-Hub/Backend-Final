using System.ComponentModel.DataAnnotations;

namespace SellerHub.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = "customer";

        // Common fields
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Region { get; set; } = string.Empty;

        // Seller-specific
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ProductCategory { get; set; } = string.Empty;

        [MaxLength(200)]
        public string WebsiteUrl { get; set; } = string.Empty;

        [MaxLength(50)]
        public string TaxId { get; set; } = string.Empty;

        // Admin-specific
        [MaxLength(500)]
        public string ContentDescription { get; set; } = string.Empty;

        [MaxLength(200)]
        public string HowDidYouHearAboutUs { get; set; } = string.Empty;

        [MaxLength(20)]
        public string MobileNumber { get; set; } = string.Empty;
        public bool TermsAccepted { get; set; } = false;

        // Optional referral / linked accounts
        [MaxLength(100)]
        public string ReferralCode { get; set; } = string.Empty;
        public int? LinkedTo { get; set; }
    }
}
