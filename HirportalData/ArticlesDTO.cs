using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HirportalData
{
    public class ArticlesDTO
    {
        public int Id { get; set; }
        public UserDTO User { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public Boolean IsMainArticle { get; set; }
        public IList<ImageDTO> Images { get; set; }

       
        public ArticlesDTO()
        {
            Images = new List<ImageDTO>();
        }
        public override Boolean Equals(Object obj)
        {
          return (obj is ArticlesDTO dto) && Id == dto.Id;
        }
    }
}
