using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_Market.Areas.Identity.Pages.Account.Manage
{
    public class UploadFileModel : PageModel
    {
        private IHostingEnvironment _environment;
        private readonly IWebHostEnvironment webHostEnvironment;

        [TempData]
        public string Status { get; set; }

        public UploadFileModel(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task OnPostAsync()
        {
            var file = Path.Combine(webHostEnvironment.WebRootPath, "images", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
                Status = "Your image has been uploaded!";
            }

            
        }
    }
}
