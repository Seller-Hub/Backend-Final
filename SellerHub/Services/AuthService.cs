using Microsoft.EntityFrameworkCore;
using SellerHub.Data;
using SellerHub.DTOs;
using SellerHub.Models;

namespace SellerHub.Services
{
    public class AuthService(AppDbContext db) : IAuthService
    {
        public async Task<User?> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            if (await db.Users.AnyAsync(u => u.Email == dto.Email))
                return null;

            var user = new User
            {
                Role = "customer",
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Region = dto.Region,
                TermsAccepted = dto.TermsAccepted,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> RegisterSellerStep1Async(RegisterSellerStep1Dto dto)
        {
            if (await db.Users.AnyAsync(u => u.Email == dto.Email))
                return null;

            var user = new User
            {
                Role = "seller",
                Email = dto.Email,
                Region = dto.Region,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> RegisterSellerStep2Async(int userId, RegisterSellerStep2Dto dto)
        {
            var user = await db.Users.FindAsync(userId);
            if (user == null || user.Role != "seller") return null;

            user.CompanyName = dto.CompanyName;
            user.ProductCategory = dto.ProductCategory;
            user.WebsiteUrl = dto.WebsiteUrl;
            user.TaxId = dto.TaxId;
            user.TermsAccepted = dto.TermsAccepted;

            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> RegisterAdminStep1Async(RegisterAdminStep1Dto dto)
        {
            if (await db.Users.AnyAsync(u => u.Email == dto.Email))
                return null;

            var user = new User
            {
                Role = "admin",
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> RegisterAdminStep2Async(int userId, RegisterAdminStep2Dto dto)
        {
            var user = await db.Users.FindAsync(userId);
            if (user == null || user.Role != "admin") return null;

            user.ContentDescription = dto.ContentDescription;
            user.Region = dto.Region;
            user.WebsiteUrl = dto.WebsiteUrl;
            user.HowDidYouHearAboutUs = dto.HowDidYouHearAboutUs;
            user.TermsAccepted = dto.TermsAccepted;

            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(LoginDto dto)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return user;
        }


        public async Task<SellerDashboardDto> GetSellerDashboardAsync(int sellerId)
        {
            var orders = await db.Orders.Where(o => o.SellerId == sellerId).ToListAsync();
            var products = await db.Products.Where(p => p.SellerId == sellerId).ToListAsync();

            var totalSales = orders.Where(o => o.Status == "completed").Sum(o => o.TotalAmount);
            var totalOrders = orders.Count;
            var storeSessions = await db.Users.CountAsync(u => u.LinkedTo == sellerId); // misal
            var overallSales = orders.Sum(o => o.TotalAmount);
            var regionalSales = orders.Where(o => o.Region == "your-region").Sum(o => o.TotalAmount);

            var ordersOverview = new OrdersOverviewDto(
                Completed: orders.Count(o => o.Status == "completed"),
                Cancelled: orders.Count(o => o.Status == "cancelled"),
                Ongoing: orders.Count(o => o.Status == "ongoing")
            );

            var topSellingProducts = products
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .Select(p => new TopSellingProductDto(p.Name, p.TotalSold))
                .ToList();

            return new SellerDashboardDto(totalSales, totalOrders, storeSessions, overallSales, regionalSales, ordersOverview, topSellingProducts);
        }

        public async Task<User?> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await db.Users.FindAsync(userId);
            if (user == null) return null;

   
            if (!string.IsNullOrEmpty(dto.FirstName))
                user.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName))
                user.LastName = dto.LastName;
            if (!string.IsNullOrEmpty(dto.MobileNumber))
                user.MobileNumber = dto.MobileNumber;
            if (!string.IsNullOrEmpty(dto.Region))
                user.Region = dto.Region;

        
            if (user.Role == "seller")
            {
                if (!string.IsNullOrEmpty(dto.CompanyName))
                    user.CompanyName = dto.CompanyName;
                if (!string.IsNullOrEmpty(dto.WebsiteUrl))
                    user.WebsiteUrl = dto.WebsiteUrl;
               
            }
            else if (user.Role == "admin")
            {
                if (!string.IsNullOrEmpty(dto.WebsiteUrl))
                    user.WebsiteUrl = dto.WebsiteUrl;
                if (!string.IsNullOrEmpty(dto.ContentDescription))
                    user.ContentDescription = dto.ContentDescription;
               
            }

          

            await db.SaveChangesAsync();
            return user;
        }

       
    }


}

