using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IInventoryService
	{
		Task<InventoryItem> AddInventoryItem(InventoryItem item);

		Task DeleteInventoryItem(string recordId);

		Task AddPicture(string recordId, string imageFileName);

		Task RemovePicture(string recordId, string imageFileName);
	}
}
