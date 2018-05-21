using System;
using System.IO;
using System.Threading.Tasks;
using StocktakingOnline.Web.Services.Declaration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace StocktakingOnline.Web.Services.Implementation
{
	public class LocalStorageService : IStorageService
	{
		private readonly IConfiguration configuration;
		private readonly ILogger<LocalStorageService> logger;

		public LocalStorageService(IConfiguration configuration, ILogger<LocalStorageService> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
		}

		public string GetFileDownloadUrl(string fileName)
		{
			var template = configuration["CloudStorage:Local:UrlTemplate"];
			return template.Replace("{container}", configuration["CloudStorage:ImageContainer"])
						   .Replace("{fileName}", fileName);
		}

		public async Task UploadStreamToFile(string fileName, Stream stream)
		{
			if(string.IsNullOrWhiteSpace(fileName) || stream==null){
				return;
			}
			var path = Path.Combine(configuration["CloudStorage:Local:Folder"],
									configuration["CloudStorage:ImageContainer"],
									fileName);
			using(var fs = new FileStream(path, FileMode.Create))
			{
				await stream.CopyToAsync(fs);
			}
		}
	}
}
