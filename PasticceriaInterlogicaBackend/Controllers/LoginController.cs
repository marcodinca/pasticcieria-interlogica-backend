using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasticceriaInterlogicaBackend.DataAccess;

namespace PasticceriaInterlogicaBackend.Controllers
{
    public class LoginDetails
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [EnableCors("DefaultCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static string Hash(string data)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(data));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        [HttpPost]
        public IActionResult Login(LoginDetails login)
        {
            bool Match = LoginDataAccess.Login(login.Email, Hash(login.Password));
            if (Match)
            {
                return Ok();
            } else
            {
                return StatusCode(401);
            }
        }
    }
}
