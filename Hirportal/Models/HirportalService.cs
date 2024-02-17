using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class HirportalService
    {
        private readonly HirportalContext context;
        public HirportalService(HirportalContext con)
        {
            context = con;
        }
        public IEnumerable<Article> Articles => context.Articles.Include(a => a.User).Include(a => a.Pictures);
        /*public IEnumerable<Article>  GetArticles(Int32 start, Int32 end)
        {
            return Articles;
        }*/
        public Article GetArticle(Int32? ArticleId)
        {
            if (ArticleId == null) return null;

            return context.Articles
                .Include(a => a.User)
                .Include(a => a.Pictures)
                .FirstOrDefault(a => a.Id == ArticleId);
        }
        public Article GetMainArticle()
        {
            var sortedArticles = Articles.OrderByDescending(c => c.Date);
            foreach (var s in sortedArticles)
            {
                if (s.IsMainArticle) return s;
            }
            return null;
        }

        public List<Article> GetArticlesExceptMain(Int32 start, Int32 end)
        {
            var sortedArticles = Articles.OrderByDescending(c => c.Date);
            Int32 i = 0;
            List<Article> arti = new List<Article>();
            Boolean l=false;
            foreach (var s in sortedArticles)
            {

                if (i >= start && i < end)
                {
                    
                    if (!s.IsMainArticle || l)
                    {
                        arti.Add(s);
                    }
                    if (s.IsMainArticle)
                    {
                        l = true;
                    }


                }
                
                i++;
            }

            return arti;
        }
        public List<Article> GetArticles(Int32 start,Int32 end)
        {
            var sortedArticles = Articles.OrderByDescending(c => c.Date);
            Int32 i = 0;
            List<Article> arti= new List<Article>();
            foreach (var s in sortedArticles)
            {
                if (i >= start && i<end) {
                    arti.Add(s);
                    
                }
                i++;
            }

            return arti;
        }
        public List<Article> SearchInArticles(Int32 start, Int32 end,String searchTerm)
        {
            if (searchTerm.Equals("")) return GetArticles(start, end);

            var sortedArticles = Articles.OrderByDescending(c => c.Date).Where(c => c.Title.Contains(searchTerm) || c.Content.Contains(searchTerm) || c.Date.ToString().Contains(searchTerm));
            Int32 i = 0;
            List<Article> arti = new List<Article>();
            foreach (var s in sortedArticles)
            {

                if (i >= start && i < end)
                {
                    arti.Add(s);
                }
                i++;
            }

            return arti;
        }

        public Byte[] GetArticleMainImage(Int32? ArticleId)
        {

            System.Diagnostics.Debug.WriteLine("Képbetöltése");
            System.Diagnostics.Debug.WriteLine(ArticleId);
            if (ArticleId == null)
                return null;

            return context.Pictures
                .Where(image => image.ArticleId == ArticleId)
                .Select(image => image.Image)
                .FirstOrDefault();
        }
        public Byte[] GetArticleNthImages(Int32? ArticleId, Int32 n)
        {
            if (ArticleId == null)
                return null;

            return context.Pictures
                .Where(image => image.ArticleId == ArticleId)
                .Select(image => image.Image)
                .Skip(n).FirstOrDefault();
        }
    }
}
