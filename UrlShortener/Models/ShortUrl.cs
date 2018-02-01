using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class ShortUrl
    {
        [Key]
        public int Id { get; set; }

        public string OriginalUrl { get; set; }

        public string Hash { get; set; }
    }
}
