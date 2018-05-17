using System;
using StocktakingOnline.Web.Models.Domain;
using System.Collections.Generic;
namespace StocktakingOnline.Web.Models.ViewModel
{
	public class PreviewViewModel
	{
		public Job CurrentJob { get; set; }
		public List<InventoryItem> Items { get; set; }
	}
}
