using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hirportal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hirportal.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly HirportalService HirportalService;

        public ArchiveController(HirportalService Service)
        {
            HirportalService = Service;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            // a városok listája
            //ViewBag.Articles = HirportalService.GetArticles(0,0).ToArray();
        }
        public IActionResult Index(Int32 start)
        {
           
            ArchiveViewModel avm = new ArchiveViewModel();
            ViewBag.Start = start;
            ViewBag.Next = start + 20;
            ViewBag.Prev = start - 20;
            avm.Articles = HirportalService.SearchInArticles(start, start + 20, "").ToArray();
            ViewBag.Size = HirportalService.Articles.Count();
            return View("Index", avm);
           // return viw(start,avm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Int32 start, ArchiveViewModel avm)
        {
            ViewBag.Start = start;
            ViewBag.Next = start + 20;
            ViewBag.Prev = start - 20;
            avm.Articles = HirportalService.SearchInArticles(start, start + 20, avm.SearchTerm).ToArray();
            ViewBag.Size = HirportalService.Articles.Count();
            return View("Index", avm);
            //return viw(start, avm);
        }
    }
}