using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HirportalData;

namespace HirportalAdmin.Presistence
{
    class HirportalPresistence : IHirportalPresistence
    {
        private HttpClient client;

        public HirportalPresistence(String baseAddress)
        {
            client = new HttpClient(); // a szolgáltatás kliense
            client.BaseAddress = new Uri(baseAddress); // megadjuk neki a címet
        }
        public async Task<IEnumerable<UserDTO>> ReadUsersAsync()
        {
            try { 
            HttpResponseMessage response = await client.GetAsync("api/users/");
            if (response.IsSuccessStatusCode) // amennyiben sikeres a művelet
            {
                IEnumerable<UserDTO> users = await response.Content.ReadAsAsync<IEnumerable<UserDTO>>();
                return users;

            }
            else
            {
                throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
            }
        }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
        
    }
}
        public async Task<IEnumerable<ArticlesDTO>> ReadArticlesAsync()
        {
            try
            {
                // a kéréseket a kliensen keresztül végezzük
                HttpResponseMessage response = await client.GetAsync("api/articles/");
                if (response.IsSuccessStatusCode) // amennyiben sikeres a művelet
                {
                    IEnumerable<ArticlesDTO> articles = await response.Content.ReadAsAsync<IEnumerable<ArticlesDTO>>();
                    // a tartalmat JSON formátumból objektumokká alakítjuk
                    foreach (ArticlesDTO article in articles)
                    {
                        response = await client.GetAsync("api/images/article/" + article.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            article.Images = (await response.Content.ReadAsAsync<IEnumerable<ImageDTO>>()).ToList();
                        }
                    }
                    return articles;

                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<bool> CreateArticleAsync(ArticlesDTO article)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/articles/", article); // az értékeket azonnal JSON formátumra alakítjuk
                article.Id = (await response.Content.ReadAsAsync<ArticlesDTO>()).Id; // a válaszüzenetben megkapjuk a végleges azonosítót

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> UpdateArticleAsync(ArticlesDTO article)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("api/articles/", article);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<bool> DeleteArticleAsync(ArticlesDTO article)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/articles/" + article.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/acount/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode; // a művelet eredménye megadja a bejelentkezés sikeressségét
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        public async Task<Boolean> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/acount/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> CreateArticleImageAsync(ImageDTO image)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/images/", image); // elküldjük a képet
                if (response.IsSuccessStatusCode)
                {
                    image.Id = await response.Content.ReadAsAsync<Int32>(); // a válaszüzenetben megkapjuk az azonosítót
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

/// <summary>
        /// Épületkép törlése.
        /// </summary>
        /// <param name="image">Épületkép.</param>
        public async Task<Boolean> DeleteArticleImageAsync(ImageDTO image)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/images/" + image.Id);
                //return response.IsSuccessStatusCode;
                return true;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
