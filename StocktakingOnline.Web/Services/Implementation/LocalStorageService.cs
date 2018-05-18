using System;
using System.IO;
using System.Threading.Tasks;
using StocktakingOnline.Web.Services.Declaration;
namespace StocktakingOnline.Web.Services.Implementation
{
	public class LocalStorageService : IStorageService
	{
		public LocalStorageService()
		{
		}

		public string GetFileDownloadUrl(string fileName)
		{
			throw new NotImplementedException();
		}

		public Task UploadStreamToFile(string fileName, Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
