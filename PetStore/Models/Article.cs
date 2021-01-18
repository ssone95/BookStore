using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Article
    {
        public long ArticleId { get; set; }
        [Required(ErrorMessage = "Please enter a Article name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a Article name")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Please select a valid ArticleType")]
        public long ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }

        public string Tags { get; set; }
    }
}
