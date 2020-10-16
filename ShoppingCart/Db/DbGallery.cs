using Castle.Core.Configuration;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace ShoppingCart.Db
{
    public class DbGallery : DbContext
    {
        protected IConfiguration configuration;

        public DbGallery(DbContextOptions<DbGallery> options)
            : base(options)
        {
            // options like which database provider to use (e.g.
            // MS SQL, Oracle, SQL Lite, MySQL
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Wishlist>().HasAlternateKey(x => new { x.UserId, x.ProductId });
            model.Entity<Cart>().HasAlternateKey(x => new { x.UserId, x.ProductId });
            model.Entity<OrderDetail>().HasAlternateKey(x => new { x.OrderId, x.ProductId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ActivationCode> ActivationCodes { get; set; }
    }
}