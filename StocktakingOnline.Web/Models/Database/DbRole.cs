using System;
using Dapper;
namespace StocktakingOnline.Web.Models.Database
{
	[Table("Roles")]
	public class DbRole
	{
		[Key]
		public int RoleId { get; set; }
		public string RoleName { get; set; }
	}
}
