using System;
using Dapper;
namespace StocktakingOnline.Web.Models.Database
{
	[Table("InventoryItemsView")]
	public class DbViewInventoryItem:DbInventoryItem
	{
		public string DepartmentName { get; set; }
		public string DisplayName { get; set; }
	}
}
