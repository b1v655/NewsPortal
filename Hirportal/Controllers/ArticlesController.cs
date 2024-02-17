using Hirportal.Models;
using HirportalData;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Controllers
    
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private HirportalContext _context;

        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        /// <param name="context">Entitásmodell.</param>
        public ArticlesController(HirportalContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetArticles()
        {
            try
            {
                var userID = User.Identity.GetUserId();
                return Ok(_context.Articles.Include(a => a.User).Where(a => a.UserId == Int32.Parse(userID)).ToList().Select(article => new ArticlesDTO
                {
                    Id = article.Id,
                    User = new UserDTO { Id = article.User.Id, Name = article.User.Name, Password = "" },
                    Title = article.Title,
                    Date = article.Date,
                    Content = article.Content,
                    Summary = article.Summary,
                    IsMainArticle = article.IsMainArticle
                }));

                //return StatusCode(StatusCodes.Status403Forbidden);


            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetArticles(Int32 id)
        {
            try
            {

                var article = _context.Articles.Include(a => a.User).Single(b => b.Id == id);
                return Ok(new ArticlesDTO
                {
                    Id = article.Id,
                    User = new UserDTO { Id = article.User.Id, Name = article.User.Name, Password = "" },
                    Title = article.Title,
                    Date = article.Date,
                    Content = article.Content,
                    Summary = article.Summary,
                    IsMainArticle = article.IsMainArticle
                });
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostArticle([FromBody] ArticlesDTO articlesDTO)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var addedArticle = _context.Articles.Add(new Article
                {
                    UserId = Int32.Parse(userID),
                    Title = articlesDTO.Title,
                    Date = articlesDTO.Date,
                    Content = articlesDTO.Content,
                    Summary = articlesDTO.Summary,
                    IsMainArticle = articlesDTO.IsMainArticle
                });
                
                _context.SaveChanges(); // elmentjük az új épületet
                
                articlesDTO.Id = addedArticle.Entity.Id;
                return CreatedAtRoute("GetArticles", new { id = addedArticle.Entity.Id }, articlesDTO);
                // visszaküldjük a létrehozott épületet
                //return Created(Request.GetUri() + addedArticle.Entity.Id.ToString(), articlesDTO);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Authorize]
        public IActionResult PutArticle([FromBody] ArticlesDTO articlesDTO)
        {
            try
            {
                Article article = _context.Articles.FirstOrDefault(b => b.Id ==articlesDTO.Id);

                if (article == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                //article.Id = articlesDTO.Id;
                //article.UserId = articlesDTO.User.Id;
                article.Title = articlesDTO.Title;
                article.Date = articlesDTO.Date;
                article.Content = articlesDTO.Content;
                article.Summary = articlesDTO.Summary;
                article.IsMainArticle = articlesDTO.IsMainArticle;
                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteArticle(Int32 id)
        {
            try
            {
                Article article = _context.Articles.FirstOrDefault(b => b.Id == id);

                if (article == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                _context.Articles.Remove(article);
                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
