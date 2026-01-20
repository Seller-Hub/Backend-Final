using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellerHub.Services;
using System.Security.Claims;

namespace SellerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerDashboardController : ControllerBase
    {
        private readonly IAuthService _authService;

        public SellerDashboardController(IAuthService authService)
        {
            _authService = authService;
        }

        // ===========================
        // Seller Dashboard
        // ===========================

        [Authorize(Roles = "seller")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var sellerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var dashboard = await _authService.GetSellerDashboardAsync(sellerId);
            return Ok(dashboard);
        }
    }
}
