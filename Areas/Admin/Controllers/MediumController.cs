using Art.Data;
using Art.Models;
using Art.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSpace_Project.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class MediumController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MediumController(ApplicationDbContext db)
        {
            _db = db;
        }


        //GET
        public async Task<IActionResult> Index()
        {
            return View(await _db.Medium.ToListAsync());
        }


        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medium medium)
        {
            if (ModelState.IsValid)
            {
                //if valid
                _db.Medium.Add(medium);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View(medium);
        }


        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var medium = await _db.Medium.FindAsync(id);
            if (medium == null)
            {
                return NotFound();
            }
            return View(medium);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Medium medium)
        {
            if (ModelState.IsValid)
            {
                _db.Update(medium);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(medium);
        }



        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var medium = await _db.Medium.FindAsync(id);
            if (medium == null)
            {
                return NotFound();
            }
            return View(medium);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var medium = await _db.Medium.FindAsync(id);

            if (medium == null)
            {
                return View();
            }
            _db.Medium.Remove(medium);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medium = await _db.Medium.FindAsync(id);
            if (medium == null)
            {
                return NotFound();
            }

            return View(medium);
        }
    }
}
