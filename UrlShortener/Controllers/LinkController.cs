using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using UrlShortener.Models;
using UrlShortener.Repositories;

namespace UrlShortener.Controllers
{
    public class LinkController : Controller
    {
        #region Fields
        private UrlRepository _repository;
        #endregion

        #region Constructor
        public LinkController(UrlDbContext context)
        {
            _repository = new UrlRepository(context);
        }
        #endregion

        #region API Actions
        
        [HttpGet("v1/links/{hash}")]
        public IActionResult Get(string hash)
        {
            ShortUrl result = _repository.GetByHash(hash);
            if (result == null)
                return NotFound();

            return Redirect(result.OriginalUrl);
        }

        [HttpPost("v1/links")]
        public async Task<IActionResult> PostAsync([FromForm]string url)
        {
            ShortUrl result = await _repository.CreateAsync(url);
            return Json(new { hash = result.Hash });
        }

        #endregion

        #region MVC Actions

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ShortUrl();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ShortUrl model)
        {
            if (ModelState.IsValid)
            {
                ShortUrl result = await _repository.CreateAsync(model.OriginalUrl);
                if (result != null)
                    return View("Success", result);
            }
            return View(model);
        }

        #endregion

        #region Pending

        [HttpPut("v1/links/{hash}")]
        public void Put(string hash, [FromBody]string value)
        {
        }

        [HttpDelete("v1/links/{hash}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
