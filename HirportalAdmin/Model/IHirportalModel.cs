using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HirportalData;

namespace HirportalAdmin.Model
{
    public interface IHirportalModel
    {
        IReadOnlyList<ArticlesDTO> Articles { get; }
       // IReadOnlyList<UserDTO> Users { get; }

        event EventHandler<ArticlesEventArgs> ArticlesChanged;
        void CreateArticle(ArticlesDTO article);
        void UpdateArticle(ArticlesDTO article);
        void DeleteArticle(ArticlesDTO article);
        void CreateImage(Int32 articleId, Byte[] timage);
        void DeleteImage(ImageDTO image);
        Task LoadAsync();
        Task SaveAsync();
        Task<Boolean> LoginAsync(String userName, String userPassword);
        Task<Boolean> LogoutAsync();
    }
}
