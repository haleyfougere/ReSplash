using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReSplash.Data;
using ReSplash.Models;

namespace ReSplash.Pages.Photos
{
    public class IndexModel : PageModel
    {
        private readonly ReSplash.Data.ReSplashContext _context;

        public IList<Photo> Photos { get; set; } = default!;

        public IndexModel(ReSplash.Data.ReSplashContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            if (_context.Photo != null)
            {
                ///////TODO: Add where clause to get a User's Photos/////////
                Photos = await _context.Photo.OrderByDescending(a => a.PublishDate).ToListAsync();
            }
        }
    }
}
