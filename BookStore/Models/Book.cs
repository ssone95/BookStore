using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public long BookId { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public long GenreId { get; set; }
        public Genre Genre { get; set; }

        public string Tags { get; set; }
    }
}
