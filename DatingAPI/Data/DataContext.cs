using API.DatingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.DatingAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users {get;set;}
    }
}