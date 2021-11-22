using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Project_Market.Pages.toAzure
{
    public class DisplayModel : PageModel
    {
        public void onGet()
        { }
        /*private IConfiguration _configuration;
  
        public DisplayModel(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        [BindProperty]
        public List<string> ImageList { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            string blobstorageconnection = "storageconnectionstring";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("brownlechutigmailcom");
            CloudBlobDirectory dirb = container.GetDirectoryReference("filescontainers");


            BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(string.Empty,
                true, BlobListingDetails.Metadata, 100, null, null, null);
                ImageList = new List<string>();

            foreach (var blobItem in resultSegment.Results)
            {
                // A flat listing operation returns only blobs, not virtual directories.
                var blob = (CloudBlob)blobItem;
                ImageList.Add(blob.Name);
                  
            }

            //View(fileList);
            return Page();
        }
    }*/
}
