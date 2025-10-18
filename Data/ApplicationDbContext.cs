using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebIsidoraPavlovic.Models;

namespace WebIsidoraPavlovic.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebIsidoraPavlovic.Models.Proizvod> Proizvodi { get; set; }
        public DbSet<KorpaItem> Korpa { get; set; }
        public DbSet<Porudzbina> Porudzbine { get; set; }
        public DbSet<PorudzbinaStavka> PorudzbineStavke { get; set; }

    }
}
