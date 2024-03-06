using Microsoft.EntityFrameworkCore;
using WalletApi.Models;

namespace WalletApi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Users> Users { get; set; }

    }
}