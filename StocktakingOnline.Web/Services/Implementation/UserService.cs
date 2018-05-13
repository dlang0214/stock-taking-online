using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Services.Declaration;
using Dapper;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class UserService : IUserService
	{
		private readonly IDbService dbService;

		public UserService(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task<bool> SwitchToJob(DbUser user, int? jobId)
		{
			user.CurrentJobId = jobId;
			using (var db = await dbService.GetConnection())
			{
				var result = await db.UpdateAsync(user);
				//Create Sequence for this job
				var seqName = $"JobSeq_{jobId}";
				await db.ExecuteAsync(
					$@"IF NOT EXISTS (SELECT * from sys.objects WHERE name='{seqName}') 
					CREATE SEQUENCE {seqName} START WITH 1 INCREMENT BY 1;");
				return result == 1;
			}
		}
	}
}
