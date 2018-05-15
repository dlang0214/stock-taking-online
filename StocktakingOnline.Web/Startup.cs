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
			services.AddScoped<IDbService, DbService>();
			services.AddScoped<IJobService, JobService>();

			services.AddDistributedMemoryCache();
			services.AddSession();

			services.AddTransient<IUserStore<DbUser>, UserStore>();
			services.AddTransient<IRoleStore<DbRole>, RoleStore>();
			services.AddIdentity<DbUser, DbRole>().AddDefaultTokenProviders();

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
