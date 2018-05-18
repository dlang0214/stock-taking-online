using System;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using StocktakingOnline.Web.Services.Declaration;
using System.IO;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class AzureStorageService : IStorageService
	{
		private readonly IConfiguration configuration;
		private readonly CloudBlobContainer imageContainer;

		public AzureStorageService(IConfiguration configuration)
		{
			this.configuration = configuration;
			var account = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnection"));
			var client = account.CreateCloudBlobClient();
			var appName = configuration["AppName"].ToLowerInvariant();
			var imageContainerName = configuration["CloudStorage:ImageContainer"].ToLowerInvariant();
			var outputContainerName = configuration["CloudStorage:OutputContainer"].ToLowerInvariant();
			imageContainer = client.GetContainerReference($"{appName}-{imageContainerName}");
		}

		public string GetFileDownloadUrl(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return null;
			}
			var blob = imageContainer.GetBlockBlobReference(fileName);
			var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
			{
				SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
				Permissions = SharedAccessBlobPermissions.Read
			});
			return blob.Uri + sas;
		}

		public async Task UploadStreamToFile(string fileName, Stream stream)
		{
			if (string.IsNullOrWhiteSpace(fileName) || stream == null)
			{
				return;
			}
			var blob = imageContainer.GetBlockBlobReference(fileName);
			await blob.UploadFromStreamAsync(stream);
			blob.Properties.ContentType = "image/jpeg";
			await blob.SetPropertiesAsync();
		}
	}
}
