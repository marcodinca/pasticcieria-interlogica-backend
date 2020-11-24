using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasticceriaInterlogicaBackend.Models
{
    [Table("Ingredienti")]
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Um { get; set; }

        [Computed]
        public float Quantita { get; set; }
    }
}
