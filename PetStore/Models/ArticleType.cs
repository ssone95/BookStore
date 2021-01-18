using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class ArticleType
    {
        public long ArticleTypeId { get; set; }
        [Required(ErrorMessage = "Please enter a valid name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a valid description")]
        public string Description { get; set; }
    }
}
