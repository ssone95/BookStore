using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Genre
    {
        public long GenreId { get; set; }
        [Required(ErrorMessage = "Please enter a valid name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a valid description")]
        public string Description { get; set; }
    }
}
