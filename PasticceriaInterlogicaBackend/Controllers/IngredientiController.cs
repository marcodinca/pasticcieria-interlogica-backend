using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasticceriaInterlogicaBackend.DataAccess;
using PasticceriaInterlogicaBackend.Models;

namespace PasticceriaInterlogicaBackend.Controllers
{
    [EnableCors("DefaultCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientiController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Ingrediente> GetIngredienti()
        {
            return IngredientiDataAccess.GetIngredienti();
        }
        
        [HttpPut]
        public IActionResult PutIngrediente([FromBody] Ingrediente ingrediente)
        {
            IngredientiDataAccess.PutIngrediente(ingrediente);

            return Ok();
        }
        
        [HttpPost]
        public IActionResult PostIngrediente([FromBody] Ingrediente ingrediente)
        {
            IngredientiDataAccess.PostIngrediente(ingrediente);

            return Ok();
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteIngrediente([FromRoute] int id)
        {
            IngredientiDataAccess.DeleteIngrediente(id);

            return Ok();
        }
    }
}