using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [Route("v1/[controller]")]
    public class LinksController : Controller
    {
        #region Fields
        private readonly UrlDbContext _context;
        #endregion

        #region Constructor
        public LinksController(UrlDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Actions
        
        // GET v1/{short-link-hash}
        [HttpGet("{hash}")]
        public IActionResult Get(string hash)
        {
            ShortUrl shortUrl = _context.ShortUrls.FirstOrDefault(u => u.Hash == hash);
            return (shortUrl != null)
                ? Redirect(shortUrl.OriginalUrl)
                : (IActionResult)NotFound();
        }

        // POST v1/links
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm]string url)
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

            return Json(new { hash = shortUrl.Hash });
        }

        #endregion

        #region Pending

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
