using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReSplash.Data;
using ReSplash.Models;

namespace ReSplash.Pages.Photos
{
    public class CreateModel : PageModel
    {
        private readonly ReSplash.Data.ReSplashContext _context;
        private readonly IWebHostEnvironment _env;

        [BindProperty]
        public Photo Photo { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageUpload { get; set; } = default!;


        public CreateModel(ReSplash.Data.ReSplashContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            string imageFileName = DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss_") + ImageUpload.FileName;

            // Set defaults for Photo
            Photo.PublishDate = DateTime.Now;
            Photo.ImageDownloads = 0;
            Photo.ImageViews = 0;
            Photo.FileName = imageFileName;

            //Get the user  
            User? user = _context.User.Where(u => u.UserId == 1).FirstOrDefault();
            if (user != null)
            {
                Photo.User = user;
            }

            // Validate Photo
            if (!ModelState.IsValid || _context.Photo == null || Photo == null)
            {
                return Page();
            }

            // Add Photo to the dbContext
            _context.Photo.Add(Photo);

            // Save the dbContext in database
            await _context.SaveChangesAsync();

            // Save image to file system
            string filePath = Path.Combine(_env.ContentRootPath, "wwwroot/photos", imageFileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                ImageUpload.CopyTo(fileStream);
            }

            return RedirectToPage("./Index");
        }
    }
}

