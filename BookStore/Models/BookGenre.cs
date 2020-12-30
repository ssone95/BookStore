using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookGenre
    {
        public long Id { get; set; }

        public long BookId { get; set; }
        public Book Book { get; set; }

        public long GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
