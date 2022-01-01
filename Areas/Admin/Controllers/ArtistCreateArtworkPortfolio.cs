using Art.Data;
using Art.Models.ViewModels;
using Art.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ManagerUser + "," + SD.ArtistUser)]
    public class ArtistCreateArtworkPortfolio : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public ArtworkPortfolioViewModel ArtworkPortfolioVM { get; set; }

        public ArtistCreateArtworkPortfolio(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            ArtworkPortfolioVM = new ArtworkPortfolioViewModel()
            {
                Medium = _db.Medium,
                ArtworkPortfolio = new Art.Models.ArtworkPortfolio()
            };


        }

        public async Task<IActionResult> Index()
        {
            var artworkPortfolios = await _db.ArtworkPortfolio.Include(m => m.Medium).Include(m => m.ArtworkType).ToListAsync();
            return View(artworkPortfolios);
        }




        //GET - CREATE
        public IActionResult Create()
        {
            return View(ArtworkPortfolioVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            ArtworkPortfolioVM.ArtworkPortfolio.ArtworkTypeId = Convert.ToInt32(Request.Form["ArtworkTypeId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(ArtworkPortfolioVM);
            }

            _db.ArtworkPortfolio.Add(ArtworkPortfolioVM.ArtworkPortfolio);
            await _db.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var artworkPortfolioFromDb = await _db.ArtworkPortfolio.FindAsync(ArtworkPortfolioVM.ArtworkPortfolio.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, ArtworkPortfolioVM.ArtworkPortfolio.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                artworkPortfolioFromDb.Image = @"\images\" + ArtworkPortfolioVM.ArtworkPortfolio.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultArtImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + ArtworkPortfolioVM.ArtworkPortfolio.Id + ".png");
                artworkPortfolioFromDb.Image = @"\images\" + ArtworkPortfolioVM.ArtworkPortfolio.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET : Details ArtworkPortfolio
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArtworkPortfolioVM.ArtworkPortfolio = await _db.ArtworkPortfolio.Include(m => m.Medium).Include(m => m.ArtworkType).SingleOrDefaultAsync(m => m.Id == id);

            if (ArtworkPortfolioVM.ArtworkPortfolio == null)
            {
                return NotFound();
            }

            return View(ArtworkPortfolioVM);
        }



    }
}




        

