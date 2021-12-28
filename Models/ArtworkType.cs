using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Models
{
    public class ArtworkType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Artwork Type Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Artwork Medium")]
        public int MediumId { get; set; }

        [ForeignKey("MediumId")]
        public virtual Medium Medium { get; set; }
    }
}
