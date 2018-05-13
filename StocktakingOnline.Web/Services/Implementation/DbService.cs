using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StocktakingOnline.Web.Services.Declaration;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class DbService : IDbService
	{

		public DbService(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public async Task<IDbConnection> GetConnection()
		{
			var con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
			await con.OpenAsync();
			return con;
		}
	}
}
