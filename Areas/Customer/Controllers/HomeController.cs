using Art.Data;
using Art.Models;
using Art.Models.ViewModels;
using Art.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Art.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }



        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                ArtworkPortfolio = await _db.ArtworkPortfolio.Include(m => m.Medium).Include(m => m.ArtworkType).ToListAsync(),
                Medium = await _db.Medium.ToListAsync(),
                Coupon = await _db.Coupon.Where(c => c.IsActive == true).ToListAsync()
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }

            return View(IndexVM);
        }

        public async Task<IActionResult> GetByArtist(string name)
        {
            {
                IndexViewModel IndexVM = new IndexViewModel()
                {
                    ArtworkPortfolio = await _db.ArtworkPortfolio.Include(m => m.Medium)
                            .Where(x => x.Artist == name)
                            .Include(m => m.ArtworkType).ToListAsync(),
                    Medium = await _db.Medium.ToListAsync(),
                    Coupon = await _db.Coupon.Where(c => c.IsActive == true).ToListAsync()
                };

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (claim != null)
                {
                    var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                    HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
                }

                return View("Index", IndexVM);
            }
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var artworkPortfolioFromDb = await _db.ArtworkPortfolio.Include(m => m.Medium).Include(m => m.ArtworkType).Where(m => m.Id == id).FirstOrDefaultAsync();

            ShoppingCart cartObj = new ShoppingCart()
            {
                ArtworkPortfolio = artworkPortfolioFromDb,
                ArtworkPortfolioId = artworkPortfolioFromDb.Id
            };

            return View(cartObj);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart CartObject)
        {
            CartObject.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = await _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId
                                                && c.ArtworkPortfolioId == CartObject.ArtworkPortfolioId).FirstOrDefaultAsync();

                if (cartFromDb == null)
                {
                    await _db.ShoppingCart.AddAsync(CartObject);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;
                }
                await _db.SaveChangesAsync();

                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, count);

                return RedirectToAction("Index");
            }
            else
            {

                var artworkPortfolioFromDb = await _db.ArtworkPortfolio.Include(m => m.Medium).Include(m => m.ArtworkType).Where(m => m.Id == CartObject.ArtworkPortfolioId).FirstOrDefaultAsync();

                ShoppingCart cartObj = new ShoppingCart()
                {
                    ArtworkPortfolio = artworkPortfolioFromDb,
                    ArtworkPortfolioId = artworkPortfolioFromDb.Id
                };

                return View(cartObj);
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
