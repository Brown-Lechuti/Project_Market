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
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Project_Market.Areas.Identity.Pages.Account.Manage
{
              
    public class UploadFileModel : PageModel
    {
        private IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        [TempData]
        public string Status { get; set; }

        public UploadFileModel(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }


        [BindProperty]
        public List<IFormFile> files { get; set; }
        public async Task OnPostAsync()
        {
            var uploadSuccess = false;
            string uploadedUri = null;

            foreach (var formFile in files)
            {
                if (formFile.Length <= 0)
                {
                    continue;
                }

                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    await UploadToBlob(formFile.FileName, fileBytes, null);

                }

            }

        }

        private async Task<(bool, string)> UploadToBlob(string filename, byte[] imageBuffer = null, Stream stream = null)
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
            string storageConnectionString = _configuration["storageconnectionstring"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'uploadblob' and append a GUID value to it to make the name unique.
                    string userid = User.Identity.Name;
                    string newString = userid.Replace("@", string.Empty);
                    string uniqueContiner_ = newString.Replace(".", string.Empty);

                    cloudBlobContainer = cloudBlobClient.GetContainerReference(uniqueContiner_);
                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    // Set the permissions so the blobs are public. 
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);

                    if (imageBuffer != null)
                    {
                        // OPTION A: use imageBuffer (converted from memory stream)
                        await cloudBlockBlob.UploadFromByteArrayAsync(imageBuffer, 0, imageBuffer.Length);
                        Status = "Images uploaded!";
                    }
                    else if (stream != null)
                    {
                        // OPTION B: pass in memory stream directly
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                        Status = "Images uploaded!";
                    }
                    else
                    {
                        return (false, null);
                        
                        Status = "Please upload valid image file.";
                    }

                    return (true, cloudBlockBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString());

                }
                catch (StorageException ex)
                {
                    Status = "Error, Please upload valid image file.";
                    return (false, null);
                }
                finally
                {

                }
            }
            else
            {
                return (false, null);
            }

        }

    }
    /*public class UploadFileModel : PageModel
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
        public IFormFile[] Upload { get; set; } 
        public async Task OnPostAsync()
        {

            if (Upload != null)
            {
                foreach(IFormFile photo in Upload)
                {
                     var fileName = User.Identity.Name + Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);

                     var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "images", fileName);
                     using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                       
                    }
                }
                Status = "Images uploaded!";

            }
            else
            {
                Status = "Please upload valid image file.";
            }


            
        }
    }*/
}
