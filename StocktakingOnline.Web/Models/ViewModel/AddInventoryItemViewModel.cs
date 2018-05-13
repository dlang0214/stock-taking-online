using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StocktakingOnline.Web.Models.ViewModel
{
	public class AddInventoryItemViewModel
	{
		[Display(Name = "商品编号")]
		public string ProductId { get; set; }

		[Required]
		[Display(Name = "数量")]
		public uint Quantity { get; set; }

		[Required]
		[Display(Name ="资产类别")]
		public string DepartmentId { get; set; }

		[Required]
		[Display(Name ="品牌")]
		public string Brand { get; set; }

		[Required]
		[Display(Name = "型号")]
		public string Model { get; set; }

		[Display(Name ="序列号")]
		public string SerialNumber { get; set; }

		[Display(Name ="照片")]
		public IEnumerable<IFormFile> Images { get; set; }

		public List<SelectListItem> DepartmentList { get; set; }

		public List<SelectListItem> BrandList { get; set; }
	}
}
