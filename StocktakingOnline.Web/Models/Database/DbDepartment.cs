using System;
using Dapper;

namespace StocktakingOnline.Web.Models.Database
{
	[Table("Departments")]
	public class DbDepartment
	{
		[Key]
		public int DepartmentId { get; set; }

		public string DepartmentName { get; set; }

		public string DepartmentDescription { get; set; }
	}
}
