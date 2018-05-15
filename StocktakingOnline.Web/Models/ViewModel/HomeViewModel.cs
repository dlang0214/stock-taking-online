using System;
using System.Collections.Generic;
using StocktakingOnline.Web.Models.Domain;
namespace StocktakingOnline.Web.Models.ViewModel
{
	public class HomeViewModel
	{
		public List<Job> Jobs { get; set; }
		public int UserCurrentJobId { get; set; }
		public string UserDisplayName { get; set; }
	}
}
