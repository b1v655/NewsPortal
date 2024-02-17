using HirportalAdmin.Presistence;
using HirportalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HirportalAdmin.Model
{
    public class HirportalModel : IHirportalModel
    {
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private IHirportalPresistence persistence;
        private List<ArticlesDTO> articles;
        private Dictionary<ArticlesDTO, DataFlag> articlesFlags;
        //private List<UserDTO> users;
        private Dictionary<ImageDTO, DataFlag> imageFlags;

        public event EventHandler<ArticlesEventArgs> ArticlesChanged;
        public HirportalModel(IHirportalPresistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));
            this.persistence = persistence;
        }

        public IReadOnlyList<ArticlesDTO> Articles
        {
            get { return articles; }
        }
       /* public IReadOnlyList<UserDTO> Users
        {
            get { return users; }
        }*/


        public void CreateArticle(ArticlesDTO article)
        {
            //Console.WriteLine("Bent:" + article.Images.Count);
            if (article == null)
                throw new ArgumentNullException(nameof(article));
            if (articles.Contains(article))
                throw new ArgumentException("The article is already in the collection.", nameof(article));

            article.Id = (articles.Count > 0 ? articles.Max(b => b.Id) : 0) + 1; // generálunk egy új, ideiglenes azonosítót (nem fog átkerülni a szerverre)
            articlesFlags.Add(article, DataFlag.Create);
            articles.Add(article);
           /* Console.WriteLine(article.Images.Count);
            IList<ImageDTO> Images = new List<ImageDTO>();
            foreach(ImageDTO image in article.Images.ToList())
            {
                Images.Add(new ImageDTO { Image = image.Image });
            }
            article.Images.Clear();
            Console.WriteLine(article.Images.Count);
            foreach (ImageDTO image in Images)
            {
                CreateImage(article.Id, image.Image);
            }*/
        }

        public void UpdateArticle(ArticlesDTO article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            // keresés azonosító alapján
            ArticlesDTO articleToModify = articles.FirstOrDefault(b => b.Id == article.Id);
            
            if (articleToModify == null)
                throw new ArgumentException("The article does not exist.", nameof(article));

            // módosítások végrehajtása
            articleToModify.User = article.User;
            articleToModify.Title = article.Title;
            articleToModify.Date = article.Date;
            articleToModify.Content = article.Content;
            articleToModify.Summary = article.Summary;
            articleToModify.IsMainArticle = article.IsMainArticle;
            //articleToModify.Images = article.Images;

            /*foreach (ImageDTO image in articleToModify.Images)
            {
                DeleteImage(image);
            }*/
            // külön állapottal jelezzük, ha egy adat újonnan hozzávett
            if (articlesFlags.ContainsKey(articleToModify) && articlesFlags[articleToModify] == DataFlag.Create)
            {
                articlesFlags[articleToModify] = DataFlag.Create;
            }
            else
            {
                articlesFlags[articleToModify] = DataFlag.Update;
            }
            /*foreach (ImageDTO image in article.Images)
            {
                CreateImage(article.Id,image.Image);
            }*/
            OnArticleChanged(article.Id);
        }

        public void DeleteArticle(ArticlesDTO article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            // keresés azonosító alapján
            ArticlesDTO articleToDelete = articles.FirstOrDefault(b => b.Id == article.Id);

            if (articleToDelete == null)
                throw new ArgumentException("The article does not exist.", nameof(article));

            // külön kezeljük, ha egy adat újonnan hozzávett (ekkor nem kell törölni a szerverről)
            if (articlesFlags.ContainsKey(articleToDelete) && articlesFlags[articleToDelete] == DataFlag.Create)
                articlesFlags.Remove(articleToDelete);
            else
                articlesFlags[articleToDelete] = DataFlag.Delete;
            //toruljuk a képeket
            if (article.Id !=0)
            {//uj kep uj cikkben;
                foreach (ImageDTO image in articleToDelete.Images.ToList())
                {
                    //Console.WriteLine("Nem jo");
                    DeleteImage(image);
                }
            }
            articles.Remove(articleToDelete);
        }

        public async Task LoadAsync()
        {
            articles = (await persistence.ReadArticlesAsync()).ToList();

            // állapotjelzések
            articlesFlags = new Dictionary<ArticlesDTO, DataFlag>();
            imageFlags = new Dictionary<ImageDTO, DataFlag>();
        }

        public async Task SaveAsync()
        {
            List<ArticlesDTO> articlesToSave = articlesFlags.Keys.ToList();

            foreach (ArticlesDTO article in articlesToSave)
            {
                Boolean result = true;

                // az állapotjelzőnek megfelelő műveletet végezzük el
                switch (articlesFlags[article])
                {
                    case DataFlag.Create:
                        
                        result = await persistence.CreateArticleAsync(article);
                        foreach(ImageDTO image in article.Images.ToList())//BARKACS
                        {
                            image.ArticleId = article.Id;
                            result = await persistence.CreateArticleImageAsync(image);
                        }
                        break;
                    case DataFlag.Delete:
                        result = await persistence.DeleteArticleAsync(article);
                        break;
                    case DataFlag.Update:
                        result = await persistence.UpdateArticleAsync(article);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + articlesFlags[article] + " failed on article " + article.Id);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                articlesFlags.Remove(article);

            }
            // képek
            List<ImageDTO> imagesToSave = imageFlags.Keys.ToList();

            foreach (ImageDTO image in imagesToSave)
            {
                Boolean result = true;

                switch (imageFlags[image])
                {
                    case DataFlag.Create:
                        result = await persistence.CreateArticleImageAsync(image);
                        break;
                    case DataFlag.Delete:
                        Console.WriteLine("DeletImageID: "+ image.Id);
                        result = await persistence.DeleteArticleImageAsync(image);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + imageFlags[image] + " failed on image " + image.Id);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                imageFlags.Remove(image);
            }
        }
        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            // IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);
            return await persistence.LoginAsync(userName, userPassword);
        }

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        public async Task<Boolean> LogoutAsync()
        {
            /* if (!IsUserLoggedIn)
                 return true;

             IsUserLoggedIn = !(await _persistence.LogoutAsync());

             return IsUserLoggedIn;*/
            return !(await persistence.LogoutAsync());
        }
        private void OnArticleChanged(Int32 articleId)
        {
            if (ArticlesChanged != null)
                ArticlesChanged(this, new ArticlesEventArgs { articleId = articleId });
        }
        public void CreateImage(Int32 articleId, Byte[] timage)
        {
            ArticlesDTO article = articles.FirstOrDefault(b => b.Id == articleId);
            if (article == null)
                throw new ArgumentException("The article does not exist.", nameof(articleId));

            // létrehozzuk a képet
            ImageDTO image = new ImageDTO
            {
                Id = articles.Max(b => b.Images.Any() ? b.Images.Max(im => im.Id) : 0) + 1,
                ArticleId = articleId,
                Image=timage
            };
            //Console.WriteLine(image.ArticleId);
            // hozzáadjuk
            article.Images.Add(image);
            imageFlags.Add(image, DataFlag.Create);

            // jellezzük a változást
            //OnArticleChanged(articleId);
        }
        public void DeleteImage(ImageDTO image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            // megkeressük a képet
            foreach (ArticlesDTO article in articles)
            {
                if (!article.Images.Contains(image))
                    continue;

                // kezeljük az állapotjelzéseket is
                if (imageFlags.ContainsKey(image))
                    imageFlags.Remove(image);
                else
                    imageFlags.Add(image, DataFlag.Delete);

                // töröljük a képet
                article.Images.Remove(image);

                // jellezzük a változást
                //OnArticleChanged(article.Id);

                return;
            }

            // ha idáig eljutott, akkor nem sikerült képet törölni+
            throw new ArgumentException("The image does not exist.", nameof(image));
        }
    }
}
