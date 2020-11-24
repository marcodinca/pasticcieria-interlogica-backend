using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasticceriaInterlogicaBackend.Models
{
    [Table("Prodotti")]
    public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public double Prezzo { get; set; }
        public string Immagine { get; set; }
        public DateTime DataProduzione { get; set; }

        [Computed]
        public int Sconto { get; set; }

        [Computed]
        public double PrezzoAttuale { get; set; }

        [Computed]
        public IEnumerable<Ingrediente> Ingredienti { get; set; }
    }
}
