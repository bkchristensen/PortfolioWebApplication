
using Microsoft.EntityFrameworkCore;

namespace PortfolioWebApplication.Models
{
    public class PurchaseContext : DbContext
    {
        public PurchaseContext(DbContextOptions<PurchaseContext> options)
            : base(options)
        {
        }

        public DbSet<Purchase> PurchaseItems { get; set; }
    }
}
