using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StocktakingOnline.Web.Services.Declaration;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.Database;
using Microsoft.Extensions.Logging;
using Dapper;
using StocktakingOnline.Web.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	public class PreviewController : Controller
	{
		private readonly IInventoryService inventoryService;
		private readonly IDbService dbService;
		private readonly IJobService jobService;
		private readonly UserManager<DbUser> userManager;
		private readonly ILogger<PreviewController> logger;

		public PreviewController(IInventoryService inventoryService, IDbService dbService,
								 IJobService jobService, UserManager<DbUser> userManager, ILogger<PreviewController> logger)
		{
			this.inventoryService = inventoryService;
			this.dbService = dbService;
			this.jobService = jobService;
			this.userManager = userManager;
			this.logger = logger;
		}

		// GET: /<controller>/
		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(HttpContext.User);
			var vm = new PreviewViewModel();
			if (user.CurrentJobId == null)
			{
				vm.CurrentJob = null;
			}
			else
			{
				vm.CurrentJob = await jobService.GetJob(user.CurrentJobId.Value);
				vm.Items = await inventoryService.GetInventoryItemsOfJob(user.CurrentJobId.Value);
			}

			return View(vm);
		}
	}
}
