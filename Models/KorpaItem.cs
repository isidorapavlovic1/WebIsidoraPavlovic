using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIsidoraPavlovic.Models
{
    public class KorpaItem
    {
        [Key]
        public int Id { get; set; }

        public int ProizvodId { get; set; }
        [ForeignKey("ProizvodId")]
        public Proizvod Proizvod { get; set; }

        public string UserId { get; set; }

        [Range(1, 1000)]
        public int Kolicina { get; set; }

        public decimal Ukupno => (Proizvod?.Cena ?? 0) * Kolicina;
    }
}
