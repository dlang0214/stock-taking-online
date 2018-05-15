using System;
using Dapper;

namespace StocktakingOnline.Web.Models.Database
{
	[Table("UserRolesView")]
	public class DbViewUserRole : DbUserRole
	{
		public string RoleName { get; set; }
	}
}
