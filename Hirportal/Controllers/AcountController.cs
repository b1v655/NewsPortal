using Hirportal.Models;
using HirportalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Controllers
{

    public static class CurrentUser
    {
        public static int Id=-1;
    }
    [Route("api/[controller]")]
    public class AcountController : Controller
    {
       // private readonly HirportalContext _context;
        private readonly SignInManager<User> _signInManager;

       

        public AcountController( SignInManager<User> signInManager/*HirportalContext context*/)
        {
            _signInManager = signInManager;
            //_context = context;
        }
        [HttpGet("login/{userName}/{userPassword}")]
        public async Task<IActionResult> Login(String userName, String userPassword)
        {
            try
            {
                // bejelentkeztetjük a felhasználót
                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, false, false);
                if (!result.Succeeded) // ha nem sikerült, akkor nincs bejelentkeztetés
                    return Forbid();

                // ha sikeres volt az ellenőrzés
                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("logout")]
        [Authorize] // csak bejelentklezett felhasználóknak
        public async Task<IActionResult> Logout()
        {
            try
            {
                // kijelentkeztetjük az aktuális felhasználót
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
       /* [HttpGet("login/{userName}/{userPassword}")]
        public IActionResult Login(String userName, String userPassword)
        {
            try
            {
                //var result = _context.Users.Where(u=> u.Name==userName && u.Password==userPassword);
                var users = _context.Users;
            Int32 i = 0;
            User userka=null;
            Boolean l = false;
            while(i<users.Count() && !l)
            {
                userka = users.ToList().ElementAt(i);
                l = userka.Name == userName && userka.Password == userPassword;
                i++;
            }
            if (l)
            {
                CurrentUser.Id = userka.Id;
                return Ok();
            }
            else {
                    return Forbid();
            }
           
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        [HttpGet("logout")]
        //[Authorize] // csak bejelentklezett felhasználóknak
        public IActionResult Logout()
        {
            try
            {
                // kijelentkeztetjük az aktuális felhasználót
                CurrentUser.Id = -1;
                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }*/
    }
}
