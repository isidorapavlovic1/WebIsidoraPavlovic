using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIsidoraPavlovic.Models
{
    public enum TipProizvoda
    {
        Igrica,
        Konzola,
        Figurica,
        Drugo
    }

    public class Proizvod
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ime { get; set; }

        [StringLength(500)]
        public string Opis { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Cena { get; set; }

        [Required]
        public TipProizvoda Tip { get; set; }

        public byte[]? Slika { get; set; }

        [NotMapped]
        public IFormFile? SlikaFile { get; set; }
    }
}
