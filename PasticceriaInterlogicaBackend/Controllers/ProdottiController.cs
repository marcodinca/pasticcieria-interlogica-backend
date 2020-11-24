using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasticceriaInterlogicaBackend.DataAccess;
using PasticceriaInterlogicaBackend.Models;

namespace PasticceriaInterlogicaBackend.Controllers
{
    [EnableCors("DefaultCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdottiController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Prodotto> GetProdotti()
        {
            //Ricevo i prodotti che non sono più vecchi di 4 giorni
            IEnumerable<Prodotto> Prodotti = ProdottiDataAccess.GetProdotti().Where(p => (DateTime.Now - p.DataProduzione).TotalDays < 3);

            //Imposto lo sconto e il nuovo prezzo per i prodotti che sono invecchiati più di un giorno
            foreach(Prodotto p in Prodotti)
            {
                double GiorniPassati = (DateTime.Now - p.DataProduzione).TotalDays;

                if(GiorniPassati > 1 && GiorniPassati < 2)
                {
                    p.Sconto = 20;
                    p.PrezzoAttuale = (p.Prezzo / 100) * 80;
                } else if(GiorniPassati > 2)
                {
                    p.Sconto = 80;
                    p.PrezzoAttuale = (p.Prezzo / 100) * 20;
                }
            }

            return Prodotti;
        }
        
        [HttpPut]
        public IActionResult PutProdotto([FromBody] Prodotto prodotto)
        {
            ProdottiDataAccess.PutProdotto(prodotto);
            return Ok();
        }
        
        [HttpPost]
        public IActionResult PostProdotto([FromBody] Prodotto prodotto)
        {
            List<Ricetta> Ricette = new List<Ricetta>();
            foreach(Ingrediente i in prodotto.Ingredienti)
            {
                Ricette.Add(new Ricetta()
                {
                    IdIngrediente = i.Id,
                    IdProdotto = prodotto.Id,
                    Quantita = i.Quantita
                });
            }

            ProdottiDataAccess.PostProdotto(prodotto, Ricette);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteProdotto([FromRoute] int id)
        {
            ProdottiDataAccess.DeleteProdotto(id);
            return Ok();
        }
    }
}