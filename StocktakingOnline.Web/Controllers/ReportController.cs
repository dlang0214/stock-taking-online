using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Services.Declaration;
using StocktakingOnline.Web.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	public class ReportController : Controller
	{
		private readonly IInventoryService inventoryService;
		private readonly IJobService jobService;
		private readonly IConfiguration configuration;
		private readonly UserManager<DbUser> userManager;
		private readonly ILogger<PreviewController> logger;

		public ReportController(IInventoryService inventoryService, IJobService jobService, IConfiguration configuration,
		                        UserManager<DbUser> userManager, ILogger<PreviewController> logger)
		{
			this.inventoryService = inventoryService;
			this.jobService = jobService;
			this.configuration = configuration;
			this.userManager = userManager;
			this.logger = logger;
		}

		// GET: /<controller>/
		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(HttpContext.User);
			var vm = new ReportViewModel();
			if (user.CurrentJobId == null)
			{
				vm.CurrentJob = null;
			}
			else
			{
				vm.CurrentJob = await jobService.GetJob(user.CurrentJobId.Value);
				vm.Items = await inventoryService.GetInventoryItemsOfJob(user.CurrentJobId.Value);
			}
			vm.LocalizationConfiguration = configuration.GetSection("ReportLocalization");

			return View(vm);
		}
	}
}
