using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hirportal.Models
{
    public class HirportalContext : /*DbContext*/IdentityDbContext<User, IdentityRole<int>, int>
    {
        public HirportalContext(DbContextOptions<HirportalContext> options)
            : base(options)
        { 

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Writers");
            // A felhasználói tábla alapértelemezett neve AspNetUsers lenne az adatbázisban, de ezt felüldefiniálhatjuk.
        }
        //public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
    }
}
