using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using lab5.Models;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace lab5.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Models.Book> Book { get; set; }

        public DbSet<Models.Issuence> Issuence { get; set; }

        public DbSet<Models.Jenre> Jenre { get; set; }

        public DbSet<Models.Reader> Reader { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var options = optionsBuilder.UseSqlServer(connectionString).Options;
        }

        public void Dispose()
        {
        }

        public DbSet<lab5.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
