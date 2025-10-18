using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebIsidoraPavlovic.Data;
using WebIsidoraPavlovic.Models;

namespace WebIsidoraPavlovic.Controllers
{
    public class ProizvodiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProizvodiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            var proizvodi = await _context.Proizvodi.ToListAsync();
            return View(proizvodi);
        }

        // DETALJI
        public async Task<IActionResult> Details(int id)
        {
            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null) return NotFound();
            return View(proizvod);
        }
        [Authorize(Roles = "ADMIN")]
        // KREIRANJE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proizvod model)
        {
            if (ModelState.IsValid)
            {
                if (model.SlikaFile != null)
                {
                    using var ms = new MemoryStream();
                    await model.SlikaFile.CopyToAsync(ms);
                    model.Slika = ms.ToArray();
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [Authorize(Roles = "ADMIN")]
        // IZMENA
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null) return NotFound();
            return View(proizvod);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Proizvod model)
        {
            var proizvod = await _context.Proizvodi.FindAsync(model.Id);
            if (proizvod == null) return NotFound();

            if (ModelState.IsValid)
            {
                proizvod.Ime = model.Ime;
                proizvod.Opis = model.Opis;
                proizvod.Cena = model.Cena;
                proizvod.Tip = model.Tip;

                if (model.SlikaFile != null)
                {
                    using var ms = new MemoryStream();
                    await model.SlikaFile.CopyToAsync(ms);
                    proizvod.Slika = ms.ToArray();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        [Authorize(Roles = "ADMIN")]
        // BRISANJE
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null) return NotFound();
            return View(proizvod);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod != null)
            {
                _context.Proizvodi.Remove(proizvod);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
