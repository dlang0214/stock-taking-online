﻿using System;
using System.Collections.Generic;
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
		public List<string> ImageFiles { get; set; }
		public int DepartmentId { get; set; }
		public string AssetNumber { get; set; }
		public string SerialNumber { get; set; }
		public string Brand { get; set; }
		public string Model { get; set; }
		public string DepartmentName { get; set; }
		public string UserDisplayName { get; set; }
	}
}
