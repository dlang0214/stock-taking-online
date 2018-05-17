using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
using StocktakingOnline.Web.Services.Declaration;
using Dapper;
using StocktakingOnline.Web.Models.Database;
using System.Linq;

namespace StocktakingOnline.Web.Services.Implementation
{
	public class DepartmentService : IDepartmentService
	{
		private readonly IDbService dbService;

		public DepartmentService(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task<Department> GetDepartmentById(int departmentId)
		{
			using (var db = await dbService.GetConnection())
			{
				var dbDepartment = await db.GetAsync<DbDepartment>(departmentId);
				return dbDepartment == null ? null : new Department
				{
					DepartmentId = dbDepartment.DepartmentId,
					DepartmentName = dbDepartment.DepartmentName,
					DepartmentDescription = dbDepartment.DepartmentDescription
				};
			}
		}

		public async Task<List<Department>> GetDepartments()
		{
			using (var db = await dbService.GetConnection())
			{
				var dbDepartments = await db.GetListAsync<DbDepartment>();
				return dbDepartments.Select(d => new Department
				{
					DepartmentId = d.DepartmentId,
					DepartmentName = d.DepartmentName,
					DepartmentDescription = d.DepartmentDescription
				})
				.OrderBy(d => d.DepartmentName)
				.ToList();
			}
		}
	}
}
