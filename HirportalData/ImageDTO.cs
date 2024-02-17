using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HirportalData
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Byte[] Image { get; set; }
    }
}
