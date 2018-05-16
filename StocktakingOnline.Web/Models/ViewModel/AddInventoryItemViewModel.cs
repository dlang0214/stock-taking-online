using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StocktakingOnline.Web.Models.ViewModel
{
	public class AddInventoryItemViewModel
	{
		[Required]
		[Display(Name = "商品编号")]
		public string ProductId { get; set; }

		[Required]
		[Display(Name = "数量")]
		public uint Quantity { get; set; }

		[Display(Name ="照片")]
		public IEnumerable<IFormFile> Images { get; set; }
	}
}
