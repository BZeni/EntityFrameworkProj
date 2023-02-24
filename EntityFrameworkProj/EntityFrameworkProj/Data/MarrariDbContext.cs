using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkProj.Data
{
    public class MarrariDbContext : DbContext
    {      
        public virtual DbSet<Lotes> Lotes { get; set; }
        public virtual DbSet<Pecas> Pecas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=MarrariDb;Trusted_connection=True;Connection Timeout=5;Encrypt=False");
        }
    }
}
