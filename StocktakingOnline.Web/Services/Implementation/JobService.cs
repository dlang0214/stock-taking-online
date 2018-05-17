using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
using StocktakingOnline.Web.Services.Declaration;
using Dapper;
using StocktakingOnline.Web.Models.Database;
using System.Linq;
using System.Net.Http.Headers;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class JobService : IJobService
	{
		private readonly IDbService dbService;

		public JobService(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task<Job> GetJob(int jobId)
		{
			using (var db = await dbService.GetConnection())
			{
				var dbJob = await db.GetAsync<DbJob>(jobId);
				if (dbJob == null) return null;
				return new Job
				{
					JobId = dbJob.JobId,
					JobName = dbJob.JobName,
					JobDescription = dbJob.JobDescription,
					IsOpened = dbJob.IsOpened
				};
			}
		}

		public async Task<List<Job>> GetJobs()
		{
			using (var db = await dbService.GetConnection())
			{
				var jobs = await db.GetListAsync<DbJob>();
				return jobs.Select(j => new Job
				{
					JobId = j.JobId,
					JobName = j.JobName,
					JobDescription = j.JobDescription,
					IsOpened = j.IsOpened
				})
			   .OrderBy(j => j.IsOpened ? 0 : 1)
			   .ThenBy(j => j.JobName)
			   .ToList();
			}
		}
	}
}
