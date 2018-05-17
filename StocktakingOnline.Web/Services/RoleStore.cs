using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Services.Declaration;
using Dapper;
using System.Linq;

namespace StocktakingOnline.Web.Services
{
	public class RoleStore : IRoleStore<DbRole>
	{
		private readonly IDbService dbService;

		public RoleStore(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task<IdentityResult> CreateAsync(DbRole role, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var result = await db.InsertAsync(role);
				return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
			}
		}

		public async Task<IdentityResult> DeleteAsync(DbRole role, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var result = await db.DeleteAsync(role);
				return result == 1 ? IdentityResult.Success : IdentityResult.Failed();
			}
		}

		public void Dispose()
		{
			//nothing to dispose
		}

		public async Task<DbRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			if (int.TryParse(roleId, out int id))
			{
				using (var db = await dbService.GetConnection())
				{
					return await db.GetAsync<DbRole>(id);
				}
			}
			else
			{
				return null;
			}
		}

		public async Task<DbRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var users = await db.GetListAsync<DbRole>(new { RoleName = normalizedRoleName });
				return users.FirstOrDefault();
			}
		}

		public Task<string> GetNormalizedRoleNameAsync(DbRole role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.RoleName);
		}

		public Task<string> GetRoleIdAsync(DbRole role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.RoleId.ToString());
		}

		public Task<string> GetRoleNameAsync(DbRole role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.RoleName);
		}

		public Task SetNormalizedRoleNameAsync(DbRole role, string normalizedName, CancellationToken cancellationToken)
		{
			role.RoleName = normalizedName;
			return Task.CompletedTask;
		}

		public Task SetRoleNameAsync(DbRole role, string roleName, CancellationToken cancellationToken)
		{
			role.RoleName = roleName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(DbRole role, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var result = await db.UpdateAsync(role);
				return result == 1 ? IdentityResult.Success : IdentityResult.Failed();
			}
		}
	}
}
