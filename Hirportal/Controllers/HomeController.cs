using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hirportal.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hirportal.Controllers
{
    public class HomeController : Controller
    {
        private readonly HirportalService HirportalService;

        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        public HomeController(HirportalService Service)
        {
            HirportalService = Service;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            // a városok listája
            //ViewBag.Articles = HirportalService.GetArticles(0,0).ToArray();
        }
        public FileResult NthImageForArticle(Int32? ArticleId, Int32 n)
        {
            Byte[] imageContent = HirportalService.GetArticleNthImages(ArticleId,n);
            if (imageContent == null) // amennyiben nem sikerült betölteni, egy alapértelmezett képet adunk vissza
                return File("~/images/neptun.png", "image/png");

            return File(imageContent, "image/png");
        }
        public FileResult ImageForArticle(Int32? ArticleId)
        {


            Byte[] imageContent = HirportalService.GetArticleMainImage(ArticleId);

            if (imageContent == null) // amennyiben nem sikerült betölteni, egy alapértelmezett képet adunk vissza
                return File("~/images/neptun.png", "image/png");
                
            return File(imageContent, "image/png");
        }
        public IActionResult Index()
        {
            // a városok listája
            ViewBag.MainArticle = HirportalService.GetMainArticle();
            return View("Index", HirportalService.GetArticlesExceptMain(0,10).ToArray());
        }
        public IActionResult Details(Int32? articleId)
        {
            Article article = HirportalService.GetArticle(articleId);
            return View("Details",article);
        }

        public IActionResult Images(Int32? articleId,Int32 current)
        {
            System.Diagnostics.Debug.WriteLine("Current1: " + current);
            Article article = HirportalService.GetArticle(articleId);

            if (current < 0) current = 0;
            if (current >= article.Pictures.Count) current = article.Pictures.Count - 1;
            ViewBag.CurrentImage = current;
            ViewBag.Next = current + 1;
            ViewBag.Prev = current - 1;
            System.Diagnostics.Debug.WriteLine("Current2: "+current);
            
            return View("Images",article);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
