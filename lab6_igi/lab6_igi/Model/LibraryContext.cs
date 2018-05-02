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

        public LibraryContext()
        {
            Database.EnsureCreated();
        }

    public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        { }
    }
}
