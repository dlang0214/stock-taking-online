using System;
using System.IO;
using System.Threading.Tasks;

namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IStorageService
	{
		Task UploadStreamToFile(string fileName, Stream stream);

		string GetFileDownloadUrl(string fileName);
	}
}
