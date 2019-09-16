using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace Poc.Retail.CrossCutting.Utils
{
    public static class AzureConnect
    {
        /// <summary>
        /// Get file in azure and create in local
        /// </summary>
        /// <returns></returns>        
        public static async Task DownloadBlobAnonymously(string urlHosted, string urlTemp)
        {
            CloudBlockBlob blob = new CloudBlockBlob(new Uri(urlHosted));
            await blob.DownloadToFileAsync(urlTemp, System.IO.FileMode.Create);
        }
    }
}
