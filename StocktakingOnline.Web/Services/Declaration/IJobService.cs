using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using StocktakingOnline.Web.Models.Domain;
namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IJobService
	{
		Task<List<Job>> GetJobs();
	}
}
