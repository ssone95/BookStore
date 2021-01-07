using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public long BookId { get; set; }
        [Required(ErrorMessage = "Please enter a book name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a book name")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Please select a valid genre")]
        public long GenreId { get; set; }
        public Genre Genre { get; set; }

        public string Tags { get; set; }
    }
}
