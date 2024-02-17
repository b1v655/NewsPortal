using Hirportal.Models;
using HirportalData;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Controllers
{
    [Route("api/images")]
    public class ArticleImagesController : Controller
    {
        private readonly HirportalContext _context;

        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        /// <param name="context">Entitásmodell.</param>
        public ArticleImagesController(HirportalContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }


        /*[HttpGet("{articleId}")]
        public IActionResult GetImages(Int32 articleId)
        {
            // a megfelelő képeket betöltjük és átalakítjuk (csak a kis képeket)
            return Ok(_context.Pictures.Where(image => image.ArticleId == articleId).Select(image => new ImageDTO { Id = image.Id, ArticleId = image.ArticleId, Image = image.Image }));
        }*/

        /// <summary>
        /// Egy kép lekérdezése.
        /// </summary>
        [HttpGet("article/{id}")]
        public IActionResult GetImage(Int32 id)
        {
            return Ok(_context.Pictures.Where(image => image.ArticleId == id).Select(image => new ImageDTO { Id = image.Id, ArticleId = image.ArticleId, Image = image.Image }));

        }

        /// <summary>
        /// Kép feltöltése.
        /// </summary>
        /// <param name="image">Kép.</param>
        [HttpPost] // itt nem kell paramétereznünk, csak jelezzük, hogy az egyedi útvonalat vesszük igénybe
        //[Authorize]
        public IActionResult PostImage([FromBody] ImageDTO image)
        {
            if (image == null || !_context.Articles.Any(article => image.ArticleId == article.Id))
                return NotFound();

            Picture articleImage = new Picture
            {
                ArticleId = image.ArticleId,
                Image = image.Image,
            };

            _context.Pictures.Add(articleImage);

            try
            {
                _context.SaveChanges();
                return Created(Request.GetUri() + image.Id.ToString(), image.Id); // csak az azonosítót küldjük vissza
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Kép törlése.
        /// </summary>
        /// <param name="id">A kép azonosítója.</param>
        [Route("{id}")]
        //[Authorize]
        public IActionResult DeleteImage(Int32 id)
        {
            Picture image = _context.Pictures.FirstOrDefault(im => im.Id == id);

            if (image == null)
                return NotFound();

            try
            {
                _context.Pictures.Remove(image);
                _context.SaveChanges();
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
