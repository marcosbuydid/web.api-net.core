using Demo.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebAPI.DatabaseContext {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public ApplicationDbContext() {}

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}
