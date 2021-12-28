using Art.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Art.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Medium> Medium { get; set; }
        public DbSet<ArtworkType> ArtworkType { get; set; }
        public DbSet<ArtworkPortfolio> ArtworkPortfolio { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<SearchByArtist> SearchByArtists { get; set; }
    }
}
