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
		[MaxLength(50, ErrorMessage = "商品编号不能多于50个字符")]
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
		[MaxLength(50, ErrorMessage = "型号长度不能多于50个字符")]
		public string Model { get; set; }

		[Display(Name ="序列号")]
		[MaxLength(50, ErrorMessage = "序列号长度不能多于50个字符")]
		public string SerialNumber { get; set; }

		[Display(Name ="照片")]
		public IEnumerable<IFormFile> Images { get; set; }

		public List<SelectListItem> DepartmentList { get; set; }

		public List<SelectListItem> BrandList { get; set; }
	}
}
