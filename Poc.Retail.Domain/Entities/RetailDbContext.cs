using Microsoft.EntityFrameworkCore;

namespace Poc.Retail.Domain.Entities
{
    /// <summary>
    /// RetailDbContext
    /// </summary>
    public partial class RetailDbContext : DbContext
    {
        public RetailDbContext()
        {
        }

        public RetailDbContext(DbContextOptions<RetailDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StockDetail> StockDetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // optionsBuilder.UseSqlServer("BBDD-CONNECTION");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<StockDetail>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.PointOfSale)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .IsUnicode(false);
            });
        }
    }
}
