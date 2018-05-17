using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StocktakingOnline.Web.Services.Declaration;
using StocktakingOnline.Web.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace StocktakingOnline.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IStorageService, StorageService>();

			services.AddScoped<IDbService, DbService>();
			services.AddScoped<IJobService, JobService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IInventoryService, InventoryService>();
			services.AddScoped<IDepartmentService, DepartmentService>();

			services.AddDistributedMemoryCache();
			services.AddSession();

			services.AddTransient<IUserStore<DbUser>, UserStore>();
			services.AddTransient<IRoleStore<DbRole>, RoleStore>();

			services.AddIdentity<DbUser, DbRole>(options =>
			{
				// Password settings
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 2;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			})
			.AddDefaultTokenProviders();

			//Configure cookie if necessary
			//services.ConfigureApplicationCookie(options=>{});

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			GlobalDiagnosticsContext.Set("nlogDbConnection", Configuration.GetConnectionString("DefaultConnection"));
			GlobalDiagnosticsContext.Set("appName", Configuration["AppName"]);
			loggerFactory.AddNLog();

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
