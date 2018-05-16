using System;
namespace StocktakingOnline.Web.Models.Domain
{
	public class InventoryItem
	{
		public string RecordId { get; set; }
		public string ProductId { get; set; }
		public decimal Quantity { get; set; }
		public int JobId { get; set; }
		public int UserId { get; set; }
		public DateTime CreatedTime { get; set; }
		public string[] ImageFiles { get; set; }
	}
}
