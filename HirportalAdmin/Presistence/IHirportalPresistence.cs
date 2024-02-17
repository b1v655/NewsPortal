using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HirportalData;
namespace HirportalAdmin.Presistence
{
    public interface IHirportalPresistence
    {
        Task<IEnumerable<ArticlesDTO>> ReadArticlesAsync();
        Task<IEnumerable<UserDTO>> ReadUsersAsync();
        Task<Boolean> CreateArticleAsync(ArticlesDTO article);
        Task<Boolean> UpdateArticleAsync(ArticlesDTO article);

        Task<Boolean> DeleteArticleAsync(ArticlesDTO article);
        Task<Boolean> LoginAsync(String userName, String userPassword);
        Task<Boolean> CreateArticleImageAsync(ImageDTO image);
        Task<Boolean> DeleteArticleImageAsync(ImageDTO image);
        Task<Boolean> LogoutAsync();
        
    }
}
