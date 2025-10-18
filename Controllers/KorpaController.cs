using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebIsidoraPavlovic.Data;
using WebIsidoraPavlovic.Models;

namespace WebIsidoraPavlovic.Controllers
{
    [Authorize(Roles = "USER")]
    public class KorpaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorpaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // PRIKAZ KORPE
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var korpa = await _context.Korpa
                .Include(k => k.Proizvod)
                .Where(k => k.UserId == userId)
                .ToListAsync();

            ViewBag.Ukupno = korpa.Sum(k => k.Ukupno);
            return View(korpa);
        }

        // DODAJ U KORPU
        [HttpPost]
        public async Task<IActionResult> DodajUKorpu(int proizvodId, int kolicina = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postojeci = await _context.Korpa
                .FirstOrDefaultAsync(k => k.ProizvodId == proizvodId && k.UserId == userId);

            if (postojeci != null)
            {
                postojeci.Kolicina += kolicina;
            }
            else
            {
                var stavka = new KorpaItem
                {
                    ProizvodId = proizvodId,
                    Kolicina = kolicina,
                    UserId = userId
                };
                _context.Korpa.Add(stavka);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Korpa");
        }

        // UKLONI IZ KORPE
        public async Task<IActionResult> Obrisi(int id)
        {
            var stavka = await _context.Korpa.FindAsync(id);
            if (stavka != null)
            {
                _context.Korpa.Remove(stavka);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // POTVRDI PORUDZBINU
        [HttpPost]
        public async Task<IActionResult> PotvrdiPorudzbinu()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var korpa = await _context.Korpa
                .Include(k => k.Proizvod)
                .Where(k => k.UserId == userId)
                .ToListAsync();

            if (!korpa.Any()) return RedirectToAction("Index");

            var porudzbina = new Porudzbina
            {
                UserId = userId,
                UkupnaCena = korpa.Sum(k => k.Ukupno)
            };

            _context.Porudzbine.Add(porudzbina);
            await _context.SaveChangesAsync();

            foreach (var item in korpa)
            {
                _context.PorudzbineStavke.Add(new PorudzbinaStavka
                {
                    PorudzbinaId = porudzbina.Id,
                    ProizvodId = item.ProizvodId,
                    Kolicina = item.Kolicina,
                    Cena = item.Proizvod.Cena
                });
            }

            _context.Korpa.RemoveRange(korpa);
            await _context.SaveChangesAsync();

            return RedirectToAction("MojePorudzbine");
        }

        // ISTORIJA PORUDZBINA
        public async Task<IActionResult> MojePorudzbine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var porudzbine = await _context.Porudzbine
                .Include(p => p.Stavke)
                .ThenInclude(s => s.Proizvod)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.DatumPorudzbine)
                .ToListAsync();

            return View(porudzbine);
        }

        [HttpPost]
        public async Task<JsonResult> AzurirajKolicinu(int id, int novaKolicina)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.Korpa
                .Include(k => k.Proizvod)
                .FirstOrDefaultAsync(k => k.Id == id && k.UserId == userId);

            if (item != null && novaKolicina > 0)
            {
                item.Kolicina = novaKolicina;
                await _context.SaveChangesAsync();
            }

            var korpa = await _context.Korpa
                .Include(k => k.Proizvod)
                .Where(k => k.UserId == userId)
                .ToListAsync();

            return Json(new
            {
                stavkaUkupno = item.Ukupno.ToString("N2"),
                ukupnoKorpa = korpa.Sum(k => k.Ukupno).ToString("N2")
            });
        }

    }
}
