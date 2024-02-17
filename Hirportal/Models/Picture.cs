using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class Picture
    {

        public int Id { get; set; }
        public int ArticleId { get; set; }
        //public byte[] Image { get; set; }
        public Byte[] Image { get; set; }
        public Article Article { get; set; }

    }
}

