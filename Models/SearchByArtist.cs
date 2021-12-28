using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Models
{
    public class SearchByArtist
    {
        [Key]
        public string Name { get; set; }
    }
}
