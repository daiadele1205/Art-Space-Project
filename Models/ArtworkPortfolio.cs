using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Models
{
    public class ArtworkPortfolio
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Artist { get; set; }

        public string Image { get; set; }

        [Display(Name = "Medium")]
        public int MediumId { get; set; }

        [ForeignKey("MediumId")]
        public virtual Medium Medium { get; set; }

        [Display(Name = "ArtworkType")]
        public int ArtworkTypeId { get; set; }

        [ForeignKey("ArtworkTypeId")]
        public virtual ArtworkType ArtworkType { get; set; }

        public string Email { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = " Price should be greater than ${1}")]
        public double Price { get; set; }

      
    }
}
