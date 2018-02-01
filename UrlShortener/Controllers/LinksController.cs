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
        private readonly UrlDbContext _context;
            
        public LinksController(UrlDbContext context)
        {
            _context = context;
        }

        // GET v1/{short-link-hash}
        [HttpGet("{hash}")]
        public IActionResult Get(int hash)
        {
            //TODO: Retrieve the original URL from the Database
            string originalUrl = "http://contoso.com";
            return Redirect(originalUrl);
        }

        // POST api/links
        [HttpPost]
        public async Task<IActionResult> PostAsync(string url)
        {
            // Check if the posted URL has already been saved.
            var entity = _context.ShortUrls.FirstOrDefault(u => u.OriginalUrl == url);
            if (entity == null)
            {
                // Create a new entry and save it int the DB.
                entity = new ShortUrl { OriginalUrl = url };
                await _context.SaveChangesAsync();
                // Refresh it from the DB with the ID already populated.
                entity = _context.ShortUrls.FirstOrDefault(u => u.OriginalUrl == url);
            }

            byte[] bytes = BitConverter.GetBytes(entity.Id);
            entity.Hash = Convert.ToBase64String(bytes);
            await _context.SaveChangesAsync();

            return Json(new { hash = entity.Hash });
        }

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
