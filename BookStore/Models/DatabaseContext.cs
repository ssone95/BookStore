using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            // Leaving empty on purpose
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<BookGenre> BookGenres { get; set; }
    }
}
