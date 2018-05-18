using System;
using StocktakingOnline.Web.Models.Domain;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
namespace StocktakingOnline.Web.Models.ViewModel
{
	public class ReportViewModel
	{
		public Job CurrentJob { get; set; }
		public List<InventoryItem> Items { get; set; }
		public IConfigurationSection LocalizationConfiguration { get; set; }
	}
}
