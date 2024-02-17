//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Hirportal.Models
{
    public class User  : IdentityUser<int>
    {
        public User()
        {
            Articles = new HashSet<Article>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        //public string Password { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
