using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Services.Declaration;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.ViewModel;
using StocktakingOnline.Web.Models.Domain;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	[Route("Inventory")]
	public class InventoryController : Controller
	{
		private readonly IInventoryService inventoryService;
		private readonly UserManager<DbUser> userManager;
		private readonly IJobService jobService;
		private readonly ILogger<InventoryController> logger;

		public InventoryController(IInventoryService inventoryService, UserManager<DbUser> userManager,
								   IJobService jobService, ILogger<InventoryController> logger)
		{
			this.inventoryService = inventoryService;
			this.userManager = userManager;
			this.jobService = jobService;
			this.logger = logger;
		}

		[HttpGet]
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(HttpContext.User);
			var vm = new InventoryViewModel();
			vm.CurrentJob = user.CurrentJobId == null ? null : await jobService.GetJob(user.CurrentJobId.Value);
			vm.AddInventoryItemViewModel = new AddInventoryItemViewModel();
			return View(vm);
		}

		[HttpPost]
		[Route("AddItem")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddItem(AddInventoryItemViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.GetUserAsync(HttpContext.User);
				if (user.CurrentJobId != null)
				{
					var item = new InventoryItem
					{
						UserId = user.UserId,
						JobId = user.CurrentJobId.Value,
						ProductId = viewModel.ProductId,
						Quantity = Convert.ToDecimal(viewModel.Quantity)
					};
					item = await inventoryService.AddInventoryItem(item);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "当前工作已结束");
				}
			}

			return RedirectToAction("Index");
		}
	}
}
