using Art.Data;
using Art.Models;
using Art.Models.ViewModels;
using Art.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art.Areas.Admin.Controllers
{

    [Area("Admin")]

    [Authorize(Roles = SD.ManagerUser)]
    //[Authorize(Roles = SD.ManagerUser + "," + SD.ArtistUser)]
    public class ArtworkTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public ArtworkTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Get INDEX
        public async Task<IActionResult> Index()
        {
            var artworkTypes = await _db.ArtworkType.Include(s => s.Medium).ToListAsync();
            return View(artworkTypes);
        }

        //GET - CREATE
        public async Task<IActionResult> Create()
        {
            ArtworkTypeAndMediumViewModel model = new ArtworkTypeAndMediumViewModel()
            {
                MediumList = await _db.Medium.ToListAsync(),
                ArtworkType = new Art.Models.ArtworkType(),
                ArtworkTypeList = await _db.ArtworkType.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtworkTypeAndMediumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesArtworkTypeExists = _db.ArtworkType.Include(s => s.Medium).Where(s => s.Name == model.ArtworkType.Name && s.Medium.Id == model.ArtworkType.MediumId);

                if (doesArtworkTypeExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Artwork Type exists under " + doesArtworkTypeExists.First().Medium.Name + " medium. Please use another name.";
                }
                else
                {
                    _db.ArtworkType.Add(model.ArtworkType);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ArtworkTypeAndMediumViewModel modelVM = new ArtworkTypeAndMediumViewModel()
            {
                MediumList = await _db.Medium.ToListAsync(),
                ArtworkType = model.ArtworkType,
                ArtworkTypeList = await _db.ArtworkType.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }


        [ActionName("GetArtworkType")]
        public async Task<IActionResult> GetArtworkType(int id)
        {
            List<ArtworkType> artworkTypes = new List<ArtworkType>();

            artworkTypes = await (from artworkType in _db.ArtworkType
                                  where artworkType.MediumId == id
                                  select artworkType).ToListAsync();
            return Json(new SelectList(artworkTypes, "Id", "Name"));
        }


        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artworkType = await _db.ArtworkType.SingleOrDefaultAsync(m => m.Id == id);

            if (artworkType == null)
            {
                return NotFound();
            }

            ArtworkTypeAndMediumViewModel model = new ArtworkTypeAndMediumViewModel()
            {
                MediumList = await _db.Medium.ToListAsync(),
                ArtworkType = artworkType,
                ArtworkTypeList = await _db.ArtworkType.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArtworkTypeAndMediumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesArtworkTypeExists = _db.ArtworkType.Include(s => s.Medium).Where(s => s.Name == model.ArtworkType.Name && s.Medium.Id == model.ArtworkType.MediumId);

                if (doesArtworkTypeExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Artwork Type exists under " + doesArtworkTypeExists.First().Medium.Name + " medium. Please use another name.";
                }
                else
                {
                    var artTypFromDb = await _db.ArtworkType.FindAsync(model.ArtworkType.Id);
                    artTypFromDb.Name = model.ArtworkType.Name;

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ArtworkTypeAndMediumViewModel modelVM = new ArtworkTypeAndMediumViewModel()
            {
                MediumList = await _db.Medium.ToListAsync(),
                ArtworkType = model.ArtworkType,
                ArtworkTypeList = await _db.ArtworkType.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            //modelVM.ArtworkType.Id = id;
            return View(modelVM);
        }

        //GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artworkType = await _db.ArtworkType.Include(s => s.Medium).SingleOrDefaultAsync(m => m.Id == id);
            if (artworkType == null)
            {
                return NotFound();
            }

            return View(artworkType);
        }

        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artworkType = await _db.ArtworkType.Include(s => s.Medium).SingleOrDefaultAsync(m => m.Id == id);
            if (artworkType == null)
            {
                return NotFound();
            }

            return View(artworkType);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artworkType = await _db.ArtworkType.SingleOrDefaultAsync(m => m.Id == id);
            _db.ArtworkType.Remove(artworkType);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
