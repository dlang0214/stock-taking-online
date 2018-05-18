using System.ComponentModel.DataAnnotations;

namespace StocktakingOnline.Web.Models.ViewModel
{
	public class EditInventoryItemViewModel
	{
		[Display(Name = "商品编号")]
		public string ProductId { get; set; }

		[Required]
		[Display(Name = "数量")]
		public uint Quantity { get; set; }

		[Required]
		[Display(Name = "资产类别")]
		public int DepartmentId { get; set; }

		[Required]
		[Display(Name = "品牌")]
		public string Brand { get; set; }

		[Required]
		[Display(Name = "型号")]
		public string Model { get; set; }

		[Display(Name = "序列号")]
		public string SerialNumber { get; set; }

	}
}
