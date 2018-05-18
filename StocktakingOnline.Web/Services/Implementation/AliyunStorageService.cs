using System;
using Microsoft.Extensions.Configuration;
using Aliyun.OSS;
using Aliyun.OSS.Common.Authentication;
using StocktakingOnline.Web.Services.Declaration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class AliyunStorageService:IStorageService
	{
		private readonly IConfiguration configuration;
		private readonly ILogger<AliyunStorageService> logger;
		private readonly IOss ossClient;

		public AliyunStorageService(IConfiguration configuration, ILogger<AliyunStorageService> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
			var endPoint = configuration["CloudStorage:Aliyun:EndPoint"];
			var accessKeyId = configuration["CloudStorage:Aliyun:AccessKeyId"];
			var accessKeySecret = configuration["CloudStorage:Aliyun:AccessKeySecret"];
			ossClient = new OssClient(endPoint, accessKeyId, accessKeySecret);
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
