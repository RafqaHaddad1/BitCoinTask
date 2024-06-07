using BiTCoin2.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace BiTCoin2.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<BCT12> BCT12 { get; set; }
        public DbSet<User> UserTable { get; set; }
    }
}
