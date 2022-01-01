using Art.Data;
using Art.Models.ViewModels;
using Art.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Art.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchByArtistController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SearchByArtistController(ApplicationDbContext db)
        {
            _db = db;
        }
        //GET
        public async Task<IActionResult> Index()
        {
            var artists = await _db.ArtworkPortfolio.Select(x => x.Artist).ToArrayAsync();

            return View(artists.Distinct().ToList());
        }

    }
}
