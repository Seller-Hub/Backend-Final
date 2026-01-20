using Microsoft.EntityFrameworkCore;
using SellerHub.Models;
using System.Collections.Generic;

namespace SellerHub.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<ProductProductCategory> ProductProductCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.ProductCategoryId });

            modelBuilder.Entity<ProductProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductProductCategory>()
                .HasOne(pc => pc.ProductCategory)
                .WithMany(c => c.ProductProductCategories)
                .HasForeignKey(pc => pc.ProductCategoryId);
        }

    }


  


    }
