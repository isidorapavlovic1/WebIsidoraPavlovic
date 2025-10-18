using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIsidoraPavlovic.Models
{
    public class PorudzbinaStavka
    {
        [Key]
        public int Id { get; set; }

        public int PorudzbinaId { get; set; }
        [ForeignKey("PorudzbinaId")]
        public Porudzbina Porudzbina { get; set; }

        public int ProizvodId { get; set; }
        [ForeignKey("ProizvodId")]
        public Proizvod Proizvod { get; set; }

        public int Kolicina { get; set; }

        public decimal Cena { get; set; }
    }
}
