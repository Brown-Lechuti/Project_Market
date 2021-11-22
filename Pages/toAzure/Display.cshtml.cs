using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Project_Market.Pages.toAzure
{
    public class DisplayModel : PageModel
    {
 
        private IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public DisplayModel(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }


        [BindProperty]
        public List<string> ImageList { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userid = User.Identity.Name;
            string newString = userid.Replace("@", string.Empty);
            string uniqueContiner_ = newString.Replace(".", string.Empty);

            BlobContinuationToken continuationToken = null;
            string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=storebbl;AccountKey=0i1Fv/O3avNyTTehyTKqGcQjmuyivvDT6H9gNCx8dCmIZGNfPIqCeoIPANXTDW+WuvFqoq3pEGnjyFW7eKtmzA==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(uniqueContiner_);
            var blobs = blobClient.GetContainerReference(uniqueContiner_).ListBlobsSegmentedAsync(continuationToken);
            var result = blobs.Result;
            continuationToken = result.ContinuationToken;
            var images = result.Results.ToList();
            // CloudBlobDirectory dirb = container.GetDirectoryReference("filescontainers");


            //BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync();
            ImageList = new List<string>();

            foreach (var blobItem in images)
            {
                // A flat listing operation returns only blobs, not virtual directories.
                var blob = (CloudBlockBlob)blobItem;
                blob.Properties.ContentType = "image/jpeg";
                _ = blob.SetPropertiesAsync();
                ImageList.Add($"{blob.Uri}");
                  
            }
            //

           /* BlobContinuationToken continuationToken = null;
            var storageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            blobClient = storageAccount.CreateCloudBlobClient();
            do
            {
                var blobs = blobClient.GetContainerReference(uniqueContiner_) .ListBlobsSegmentedAsync(continuationToken);
                var result = blobs.Result;
                continuationToken = result.ContinuationToken;
                var images = result.Results.ToList();
                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    if (image.GetType() == typeof(CloudBlockBlob)
                        && Path.GetExtension(image.Uri.ToString()) == ".jpg")
                    {
                        var blob = (CloudBlockBlob)image;
                        blob.Properties.ContentType = "image/jpeg";
                        blob.SetPropertiesAsync();
                        Console.WriteLine($"{i}-{image.Uri}");
                    }
                }
            } while (continuationToken != null);*/

            return Page();
        }
    }
}
