using System;
using Dapper;
namespace StocktakingOnline.Web.Models.Database
{
	[Table("Users")]
	public class DbUser
	{
		[Key]
		public int UserId { get; set; }

		public string UserName { get; set; }

		public string DisplayName { get; set; }

		public string PasswordHash { get; set; }

		public DateTime CreatedTime { get; set; }
	
		public int? CurrentJobId { get; set; }
	}

}
