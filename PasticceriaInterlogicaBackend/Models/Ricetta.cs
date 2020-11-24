using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasticceriaInterlogicaBackend.Models
{
    [Table("Ricette")]
    public class Ricetta
    {
        public int IdProdotto { get; set; }
        public int IdIngrediente { get; set; }
        public float Quantita { get; set; }
    }
}
