using SellerHub.DTOs;
using SellerHub.Models;

namespace SellerHub.Services
{
    public interface IAuthService
    {
        // CUSTOMER
        Task<User?> RegisterCustomerAsync(RegisterCustomerDto dto);

        // SELLER
        Task<User?> RegisterSellerStep1Async(RegisterSellerStep1Dto dto);
        Task<User?> RegisterSellerStep2Async(int userId, RegisterSellerStep2Dto dto);

        // ADMIN
        Task<User?> RegisterAdminStep1Async(RegisterAdminStep1Dto dto);
        Task<User?> RegisterAdminStep2Async(int userId, RegisterAdminStep2Dto dto);

        // LOGIN
        Task<User?> LoginAsync(LoginDto dto);
        Task<SellerDashboardDto> GetSellerDashboardAsync(int sellerId);

        // UPDATE

        Task<User?> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    }
}
