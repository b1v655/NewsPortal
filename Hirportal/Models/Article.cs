using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class Article
    {
        public Article()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public Boolean IsMainArticle { get; set; }
        public ICollection<Picture> Pictures { get; set; }
        public User User { get; set; }
    }
}

