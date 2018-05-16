using System;
using StocktakingOnline.Web.Models.Domain;
using System.Collections.Generic;
using StocktakingOnline.Web.Models.Database;
namespace StocktakingOnline.Web.Models.ViewModel
{
	public class PreviewViewModel
	{
		public Job CurrentJob { get; set; }
		public List<DbViewInventoryItem> Items { get; set; }
	}
}
