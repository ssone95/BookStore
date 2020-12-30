using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Genre
    {
        public long GenreId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public long? ParentGenreId { get; set; }
        public Genre ParentGenre { get; set; }
    }
}
