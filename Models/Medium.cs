using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Models
{
    public class Medium
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Artwork Medium")]
        [Required]
        public string Name { get; set; }
    }
}
