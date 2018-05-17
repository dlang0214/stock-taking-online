using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
using System.Collections.Generic;
using StocktakingOnline.Web.Models.Database;
namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IInventoryService
	{
		Task<InventoryItem> AddInventoryItem(InventoryItem item);

		Task DeleteInventoryItem(string recordId);

		Task AddPicture(string recordId, string imageFileName);

		Task RemovePicture(string recordId, string imageFileName);

		Task<List<InventoryItem>> GetInventoryItemsOfJob(int jobId);

		Task<InventoryItem> GetLastInventoryItemOfUser(int jobId, int userId);
	}
}
