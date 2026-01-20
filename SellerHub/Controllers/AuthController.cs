using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellerHub.DTOs;
using SellerHub.Models;
using SellerHub.Services;
using System.Security.Claims;

namespace SellerHub.Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // ===========================
    // REGISTER CUSTOMER
    // ===========================
    [HttpPost("register/customer")]
    public async Task<IActionResult> RegisterCustomer(RegisterCustomerDto dto)
    {
        var user = await _authService.RegisterCustomerAsync(dto);
        if (user is null)
            return BadRequest(new { message = "User already exists" });

        return Ok(new
        {
            message = "Customer registered successfully",
            userId = user.UserId,
            firstName = user.FirstName,
            lastName = user.LastName,
            email = user.Email,
            role = user.Role
        });
    }

    // ===========================
    // REGISTER SELLER STEP1
    // ===========================
    [HttpPost("register/seller/step1")]
    public async Task<IActionResult> RegisterSellerStep1(RegisterSellerStep1Dto dto)
    {
        var user = await _authService.RegisterSellerStep1Async(dto);
        if (user is null)
            return BadRequest(new { message = "User already exists" });

        return Ok(new
        {
            message = "Seller step1 completed, continue with step2",
            userId = user.UserId,
            email = user.Email,
            role = user.Role
        });
    }

    // ===========================
    // REGISTER SELLER STEP2
    // ===========================
    [HttpPost("register/seller/step2/{userId}")]
    public async Task<IActionResult> RegisterSellerStep2(int userId, RegisterSellerStep2Dto dto)
    {
        var user = await _authService.RegisterSellerStep2Async(userId, dto);
        if (user is null)
            return BadRequest(new { message = "Invalid seller user" });

        return Ok(new
        {
            message = "Seller registered successfully",
            userId = user.UserId,
            companyName = user.CompanyName,
            productCategory = user.ProductCategory,
            role = user.Role
        });
    }

    // ===========================
    // REGISTER ADMIN STEP1
    // ===========================
    [HttpPost("register/admin/step1")]
    public async Task<IActionResult> RegisterAdminStep1(RegisterAdminStep1Dto dto)
    {
        var user = await _authService.RegisterAdminStep1Async(dto);
        if (user is null)
            return BadRequest(new { message = "User already exists" });

        return Ok(new
        {
            message = "Admin step1 completed, continue with step2",
            userId = user.UserId,
            email = user.Email,
            role = user.Role
        });
    }

    // ===========================
    // REGISTER ADMIN STEP2
    // ===========================
    [HttpPost("register/admin/step2/{userId}")]
    public async Task<IActionResult> RegisterAdminStep2(int userId, RegisterAdminStep2Dto dto)
    {
        var user = await _authService.RegisterAdminStep2Async(userId, dto);
        if (user is null)
            return BadRequest(new { message = "Invalid admin user" });

        return Ok(new
        {
            message = "Admin registered successfully",
            userId = user.UserId,
            email = user.Email,
            role = user.Role
        });
    }

    // ===========================
    // LOGIN
    // ===========================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _authService.LoginAsync(dto);

        if (user is null)
            return Unauthorized(new { message = "Invalid credentials" });

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return Ok(new
        {
            message = "Logged in successfully",
            userId = user.UserId,
            firstName = user.FirstName,
            lastName = user.LastName,
            email = user.Email,
            role = user.Role
        });
    }

    // ===========================
    // LOGOUT
    // ===========================
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logged out successfully" });
    }

    // ===========================
    // GET CURRENT USER
    // ===========================
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            message = "User info retrieved successfully",
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            email = User.FindFirst(ClaimTypes.Email)?.Value,
            role = User.FindFirst(ClaimTypes.Role)?.Value
        });
    }


    // ===========================
    // UPDATE PROFİLE
    // ===========================
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
       
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId))
            return Unauthorized(new { message = "Invalid user identifier." });

    
        var updatedUser = await _authService.UpdateProfileAsync(userId, dto);

        if (updatedUser is null)
            return NotFound(new { message = "User not found." });

      
        return Ok(new
        {
            message = "Profile updated successfully",
            userId = updatedUser.UserId,
            firstName = updatedUser.FirstName,
            lastName = updatedUser.LastName,
            email = updatedUser.Email, 
            mobileNumber = updatedUser.MobileNumber,
            region = updatedUser.Region,
            companyName = updatedUser.CompanyName, 
            websiteUrl = updatedUser.WebsiteUrl,   
            role = updatedUser.Role
        });
    }


}
