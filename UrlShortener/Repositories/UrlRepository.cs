using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UrlRepository
    {
        private readonly UrlDbContext _context;

        public UrlRepository(UrlDbContext context)
        {
            _context = context;
        }

        public async Task<ShortUrl> CreateAsync(string url)
        {
            // Check if the posted URL has already been saved.
            ShortUrl shortUrl = _context.ShortUrls.FirstOrDefault(u => u.OriginalUrl == url);
            if (shortUrl == null)
            {
                shortUrl = new ShortUrl { OriginalUrl = url };
                _context.Add(shortUrl);
                _context.SaveChanges(); // Allow the DB to generate the autoincrement ID.

                // Generate and set the hash from the Id (instead of the full URL to make it shorter).
                byte[] bytes = BitConverter.GetBytes(shortUrl.Id);
                shortUrl.Hash = Convert.ToBase64String(bytes);

                // Save again with the hash set.
                await _context.SaveChangesAsync();
            }

            return shortUrl;
        }

        public ShortUrl GetByHash(string hash)
        {
            return _context.ShortUrls.FirstOrDefault(u => u.Hash == hash);
        }
    }
}
