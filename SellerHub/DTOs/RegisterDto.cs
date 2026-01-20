namespace SellerHub.DTOs
{
    // Customer Register (single step)
    public record RegisterCustomerDto(
        string FirstName,
        string LastName,
        string Email,
        string Region,
        string Password,
        string ConfirmPassword,
        bool TermsAccepted
    );

    // Seller Register Step 1 (basic info)
    public record RegisterSellerStep1Dto(
        string Email,
        string Region,
        string Password,
        string ConfirmPassword
    );

    // Seller Register Step 2 (business info)
    public record RegisterSellerStep2Dto(
        string CompanyName,
        string ProductCategory,
        string WebsiteUrl,
        string TaxId,
        bool TermsAccepted
    );

    // Admin Register Step 1
    public record RegisterAdminStep1Dto(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ConfirmPassword
    );

    // Admin Register Step 2
    public record RegisterAdminStep2Dto(
        string ContentDescription,
        string Region,
        string WebsiteUrl,
        string HowDidYouHearAboutUs,
        bool TermsAccepted
    );


    public record UpdateProfileDto(
    string? FirstName,
    string? LastName,
    string? MobileNumber, 
    string? Region,
    string? CompanyName, 
    string? WebsiteUrl,  
    string? ContentDescription
);

}
