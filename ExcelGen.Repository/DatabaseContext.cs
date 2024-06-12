using ExcelGen.Repository.AuthorizationData;
using ExcelGen.Repository.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ExcelGen.Repository
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<Access> Access { get; set; }
    }
}
