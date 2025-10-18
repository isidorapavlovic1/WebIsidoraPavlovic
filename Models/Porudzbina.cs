using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIsidoraPavlovic.Models
{
    public class Porudzbina
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime DatumPorudzbine { get; set; } = DateTime.Now;

        public decimal UkupnaCena { get; set; }

        public ICollection<PorudzbinaStavka> Stavke { get; set; } = new List<PorudzbinaStavka>();
    }
}
