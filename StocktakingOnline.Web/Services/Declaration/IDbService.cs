using System;
using System.Data;
using System.Threading.Tasks;

namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IDbService
	{
		Task<IDbConnection> GetConnection();
	}
}
