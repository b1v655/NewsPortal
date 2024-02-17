using Hirportal.Models;
using HirportalData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Controllers
{
    [Route("api/controller")]
    public class UsersController : Controller
    {
        private HirportalContext _context;
        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        /// <param name="context">Entitásmodell.</param>
        public UsersController(HirportalContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(_context.Users.ToList().Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = ""
                }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
