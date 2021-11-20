using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Project_Market.Areas.Identity.Pages.Account.Manage
{
    public partial class UploadPhotosModel : PageModel
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UploadPhotosModel(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public List<string> ImageList { get; set; }
        public IActionResult OnGet()
        {
            var provider = new PhysicalFileProvider(webHostEnvironment.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine( "images"));
            var objFiles = contents.OrderBy(m => m.LastModified);

            ImageList = new List<string>();
            foreach (var item in objFiles.ToList())
            {
                if (item.Name.Contains(User.Identity.Name) == true)//Only return images with current user's id which their unique email address
                {
                    ImageList.Add(item.Name);
                }
                
            }
            return Page();
        }
  
    }
 }
     
    

