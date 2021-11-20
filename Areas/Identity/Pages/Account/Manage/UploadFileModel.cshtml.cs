using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_Market.Areas.Identity.Pages.Account.Manage
{
    public class UploadFileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

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

            if (Upload != null)
            {

                var fileName = User.Identity.Name + Guid.NewGuid().ToString() + "_" + Path.GetFileName(Upload.FileName);

                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "images", fileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                    Status = "Your image has been uploaded!";
                }
            }
            else
            {
                Status = "Please upload valid image file.";
            }


            
        }
    }
}
