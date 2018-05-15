using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Services.Declaration;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace StocktakingOnline.Web.Services
{
	public class UserStore : IUserStore<DbUser>, IUserPasswordStore<DbUser>, IUserRoleStore<DbUser>
	{
		private readonly IDbService dbService;

		public UserStore(IDbService dbService)
		{
			this.dbService = dbService;
		}

		public async Task AddToRoleAsync(DbUser user, string roleName, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var userRoles = await db.GetListAsync<DbViewUserRole>(new { UserId = user.UserId, RoleId = roleName });
				var userRole = userRoles.SingleOrDefault();
				if (userRole == null)
				{
					await db.InsertAsync(new DbUserRole
					{
						UserId = user.UserId,
						RoleId = userRole.RoleId
					});
				}
			}
		}

		public async Task<IdentityResult> CreateAsync(DbUser user, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var result = await db.InsertAsync(user);
				return result == 1 ? IdentityResult.Success : IdentityResult.Failed();
			}
		}

		public async Task<IdentityResult> DeleteAsync(DbUser user, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var result = await db.DeleteAsync(user);
				return result == 1 ? IdentityResult.Success : IdentityResult.Failed();
			}
		}

		public void Dispose()
		{
			//nothing to dispose
		}

		public async Task<DbUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			if (int.TryParse(userId, out int id))
			{
				using (var db = await dbService.GetConnection())
				{
					return await db.GetAsync<DbUser>(id);
				}
			}
			else
			{
				return null;
			}
		}

		public async Task<DbUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var users = await db.GetListAsync<DbUser>(new { UserName = normalizedUserName });
				return users.FirstOrDefault();
			}
		}

		public Task<string> GetNormalizedUserNameAsync(DbUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.UserName);
		}

		public Task<string> GetPasswordHashAsync(DbUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.PasswordHash);
		}

		public async Task<IList<string>> GetRolesAsync(DbUser user, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var userRoles = await db.GetListAsync<DbViewUserRole>(new { UserId = user.UserId });
				return userRoles.Select(r => r.RoleName).ToList();
			}
		}

		public Task<string> GetUserIdAsync(DbUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.UserId.ToString());
		}

		public Task<string> GetUserNameAsync(DbUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.UserName);
		}

		public async Task<IList<DbUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var userRoles = await db.GetListAsync<DbViewUserRole>(new { RoleName = roleName });
				var users = await db.GetListAsync<DbUser>("where UserId in @UserIds",
														  new { UserIds = userRoles.Select(ur => ur.UserId).ToArray() });
				return users.ToList();
			}
		}

		public Task<bool> HasPasswordAsync(DbUser user, CancellationToken cancellationToken)
		{
			return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
		}

		public async Task<bool> IsInRoleAsync(DbUser user, string roleName, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var userRoles = await db.GetListAsync<DbViewUserRole>(new { UserId = user.UserId, RoleName = roleName });
				return userRoles.Any();
			}
		}

		public async Task RemoveFromRoleAsync(DbUser user, string roleName, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var userRoles = await db.GetListAsync<DbViewUserRole>(new { UserId = user.UserId, RoleName = roleName });
				var userRole = userRoles.FirstOrDefault();
				if (userRole != null)
				{
					await db.DeleteAsync(new DbUserRole
					{
						UserId = userRole.UserId,
						RoleId = userRole.RoleId
					});
				}
			}
		}

		public Task SetNormalizedUserNameAsync(DbUser user, string normalizedName, CancellationToken cancellationToken)
		{
			user.UserName = normalizedName;
			return Task.CompletedTask;
		}

		public Task SetPasswordHashAsync(DbUser user, string passwordHash, CancellationToken cancellationToken)
		{
			user.PasswordHash = passwordHash;
			return Task.CompletedTask;
		}

		public Task SetUserNameAsync(DbUser user, string userName, CancellationToken cancellationToken)
		{
			user.UserName = userName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(DbUser user, CancellationToken cancellationToken)
		{
			using (var db = await dbService.GetConnection())
			{
				var result = await db.UpdateAsync(user);
				return result == 1 ? IdentityResult.Success : IdentityResult.Failed();
			}
		}
	}
}
