using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
using StocktakingOnline.Web.Services.Declaration;
using StocktakingOnline.Web.Models.Database;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class InventoryService : IInventoryService
	{
		private readonly IDbService dbService;

		public InventoryService(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task<InventoryItem> AddInventoryItem(InventoryItem item)
		{
			using (var db = await dbService.GetConnection())
			{
				var job = await db.GetAsync<DbJob>(item.JobId);
				var seqName = $"JobSeq_{item.JobId}";
				int nextSequenceNumber = await db.ExecuteScalarAsync<int>($@"SELECT NEXT VALUE FOR {seqName};");
				item.RecordId = GetRecordId(item.JobId, item.UserId, nextSequenceNumber);
				item.CreatedTime = DateTime.Now;
				item.AssetNumber = $"{job.JobName}{item.Brand}{nextSequenceNumber:D4}";

				var dbInventoryItem = new DbInventoryItem
				{
					RecordId = item.RecordId,
					JobId = item.JobId,
					UserId = item.UserId,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					CreatedTime = item.CreatedTime,
					ImageFiles = string.Join(";", item.ImageFiles ?? new List<string>(0)),
					Brand = item.Brand,
					Model = item.Model,
					AssetNumber = item.AssetNumber,
					DepartmentId = item.DepartmentId,
					SerialNumber = item.SerialNumber
				};
				await db.InsertAsync<string, DbInventoryItem>(dbInventoryItem);
			}

			return item;
		}

		private string GetRecordId(int jobId, int userId, int seqNumber)
		{
			return $"{jobId:D4}{userId:D4}{seqNumber:D6}";
		}

		public async Task AddPicture(string recordId, string imageFileName)
		{
			using (var db = await dbService.GetConnection())
			{
				var item = await db.GetAsync<DbInventoryItem_ImageFiles>(recordId);
				if (item == null) return;
				imageFileName = imageFileName.ToLowerInvariant();
				if (item.ImageFiles?.Contains(imageFileName) == true)
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
			using (var db = await dbService.GetConnection())
			{
				await db.DeleteAsync<DbInventoryItem>(recordId);
			}
		}

		public async Task<List<InventoryItem>> GetInventoryItemsOfJob(int jobId)
		{
			using (var db = await dbService.GetConnection())
			{
				var dbItems = await db.GetListAsync<DbViewInventoryItem>(new { JobId = jobId });
				return dbItems.Select(DbViewToDomainInventoryItem).OrderBy(item => item.CreatedTime).ToList();
			}
		}

		public async Task<InventoryItem> GetLastInventoryItemOfUser(int jobId, int userId)
		{
			using (var db = await dbService.GetConnection())
			{
				var dbItems = await db.GetListPagedAsync<DbViewInventoryItem>(1, 1,
							"where JobId=@JobId and UserId=@UserId", "CreatedTime desc",
							new { JobId = jobId, UserId = userId });
				var item = dbItems.FirstOrDefault();
				return DbViewToDomainInventoryItem(item);
			}
		}

		private static InventoryItem DbViewToDomainInventoryItem(DbViewInventoryItem item)
		{
			if (item == null) return null;
			return new InventoryItem
			{
				RecordId = item.RecordId,
				JobId = item.JobId,
				UserId = item.UserId,
				ProductId = item.ProductId,
				Quantity = item.Quantity,
				CreatedTime = item.CreatedTime,
				ImageFiles = item.ImageFiles?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(0),
				Brand = item.Brand,
				Model = item.Model,
				AssetNumber = item.AssetNumber,
				DepartmentId = item.DepartmentId,
				DepartmentName = item.DepartmentName,
				SerialNumber = item.SerialNumber,
				UserDisplayName = item.DisplayName
			};
		}
	}
}
