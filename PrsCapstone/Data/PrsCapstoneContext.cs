using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrsCapstone.Models {
    public class PrsCapstoneContext : DbContext {
        public PrsCapstoneContext(DbContextOptions<PrsCapstoneContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder model) {
            model.Entity<User>(e => {
                e.ToTable("Users");
                e.HasIndex(x => x.Username).IsUnique();
            });
        }
    }
}
