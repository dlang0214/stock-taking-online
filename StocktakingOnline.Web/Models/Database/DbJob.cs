using System;
using Dapper;
namespace StocktakingOnline.Web.Models.Database
{
	[Table("Jobs")]
	public class DbJob
	{
		[Key]
		public int JobId { get; set; }

		public string JobName { get; set; }

		public string JobDescription { get; set; }

		public bool IsOpened { get; set; }
	}
}
