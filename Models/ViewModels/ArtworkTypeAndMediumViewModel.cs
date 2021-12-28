using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Models.ViewModels
{
    public class ArtworkTypeAndMediumViewModel
    {
        public IEnumerable<Medium> MediumList { get; set; }
        public ArtworkType ArtworkType { get; set; }
        public List<string> ArtworkTypeList { get; set; }
        public string StatusMessage { get; set; }
    }
}
