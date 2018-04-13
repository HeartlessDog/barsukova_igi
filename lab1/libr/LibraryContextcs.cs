using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using libr.Models;

namespace libr
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Jenre> Jenres { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Issuence> Issuances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            //string connectionString = config.GetConnectionString("SqliteConnection");
            string connectionString = config.GetConnectionString("SQLConnection");

            var options = optionsBuilder
                .UseSqlServer(connectionString)
                //.UseSqlite(connectionString)
                .Options;
        }
    }
}
