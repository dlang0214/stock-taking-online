using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
using StocktakingOnline.Web.Services.Declaration;
using StocktakingOnline.Web.Models.Database;
using Dapper;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class InventoryService:IInventoryService
	{
		private readonly IDbService dbService;

		public InventoryService(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task<InventoryItem> AddInventoryItem(InventoryItem item)
		{
			item.RecordId = GetRecordId(item.JobId, item.UserId);
			item.CreatedTime = DateTime.Now;
			item.ImageFiles = null;

			var dbInventoryItem = new DbInventoryItem
			{
				RecordId = item.RecordId,
				JobId = item.JobId,
				UserId = item.UserId,
				ProductId = item.ProductId,
				Quantity = item.Quantity,
				CreatedTime = item.CreatedTime
			};
			using(var db = await dbService.GetConnection())
			{
				await db.InsertAsync<string, DbInventoryItem>(dbInventoryItem);
			}

			return item;
		}

		private string GetRecordId(int jobId, int userId)
		{
			return Guid.NewGuid().ToString("N");
		}

		public async Task AddPicture(string recordId, string imageFileName)
		{
			using(var db = await dbService.GetConnection())
			{
				var item = await db.GetAsync<DbInventoryItem_ImageFiles>(recordId);
				if (item == null) return;
				imageFileName = imageFileName.ToLowerInvariant();
				if(item.ImageFiles?.Contains(imageFileName) == true)
				{
					return;
				}
				if (string.IsNullOrWhiteSpace(item.ImageFiles))
				{
					item.ImageFiles = imageFileName;
				}
				else
				{
					item.ImageFiles = item.ImageFiles + ";" + imageFileName;
				}
				await db.UpdateAsync(item);
			}
		}

		public async Task RemovePicture(string recordId, string imageFileName)
		{
			using (var db = await dbService.GetConnection())
			{
				var item = await db.GetAsync<DbInventoryItem_ImageFiles>(recordId);
				if (item?.ImageFiles == null) return;
				imageFileName = imageFileName.ToLowerInvariant();
				if (item.ImageFiles.Contains(imageFileName))
				{
					var imageList = item.ImageFiles.Split(';').AsList();
					imageList.Remove(imageFileName);
					item.ImageFiles = string.Join(";", imageList);
					await db.UpdateAsync(item);
				}

			}
		}

		public async Task DeleteInventoryItem(string recordId)
		{
			using(var db = await dbService.GetConnection())
			{
				await db.DeleteAsync<DbInventoryItem>(recordId);
			}
		}
	}
}
