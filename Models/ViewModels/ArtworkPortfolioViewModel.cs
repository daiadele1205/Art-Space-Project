using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Models.ViewModels
{
    public class ArtworkPortfolioViewModel
    {
        public ArtworkPortfolio ArtworkPortfolio { get; set; }
        public IEnumerable<Medium> Medium { get; set; }
        public IEnumerable<ArtworkType> ArtworkType { get; set; }
    }
}
