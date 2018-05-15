using System;
namespace StocktakingOnline.Web.Models.Domain
{
	public class Job
	{
		public int JobId { get; set; }
		public string JobName { get; set; }
		public string JobDescription { get; set; }
		public bool IsOpened { get; set; }
	}
}
