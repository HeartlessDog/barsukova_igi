using Microsoft.EntityFrameworkCore;

namespace Lab3IGI
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-H9ELFFU;Database=LibrLab6;Trusted_Connection=True;");

        }
    }
}
