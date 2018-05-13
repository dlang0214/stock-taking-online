using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Database;
namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IUserService
	{
		Task<bool> SwitchToJob(DbUser user, int? jobId);
	}
}
