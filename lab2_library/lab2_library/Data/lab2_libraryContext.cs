using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using libr.Models;

namespace lab2_library.Models
{
    public class lab2_libraryContext : DbContext
    {
        public lab2_libraryContext (DbContextOptions<lab2_libraryContext> options)
            : base(options)
        {
        }

        public DbSet<libr.Models.Book> Book { get; set; }

        public DbSet<libr.Models.Issuence> Issuence { get; set; }

        public DbSet<libr.Models.Jenre> Jenre { get; set; }

        public DbSet<libr.Models.Reader> Reader { get; set; }
    }
}
