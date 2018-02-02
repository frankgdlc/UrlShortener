using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=short-urls.db");
        }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
