using System;
using Dapper;
namespace StocktakingOnline.Web.Models.Database
{
	[Table("UserRoles")]
	public class DbUserRole
	{
		public int UserId { get; set; }
		public int RoleId { get; set; }
	}
}
