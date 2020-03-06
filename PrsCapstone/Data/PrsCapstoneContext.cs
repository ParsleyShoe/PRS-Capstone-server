using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrsCapstone.Models;

namespace PrsCapstone.Models
    {
    public class PrsCapstoneContext : DbContext {
        public PrsCapstoneContext(DbContextOptions<PrsCapstoneContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestLine> RequestLines { get; set; }

        protected override void OnModelCreating(ModelBuilder model) {
            model.Entity<User>(e => {
                e.ToTable("Users");
                e.HasIndex(x => x.Username).IsUnique();
            });
            model.Entity<Vendor>(e => {
                e.ToTable("Vendors");
                e.HasIndex(x => x.Code).IsUnique();
            });
            model.Entity<Product>(e => {
                e.ToTable("Products");
                e.Property(x => x.Price).HasColumnType("decimal(11, 2)");
                e.HasIndex(x => x.PartNumber).IsUnique();
            });
            model.Entity<Request>(e => {
                e.ToTable("Requests");
                e.Property(x => x.Total).HasColumnType("decimal(11, 2)");
            });
        }
    }
}
