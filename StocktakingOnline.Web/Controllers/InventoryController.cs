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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	[Route("Inventory")]
	public class InventoryController : Controller
	{
		private readonly IInventoryService inventoryService;
		private readonly IDepartmentService departmentService;
		private readonly IStorageService storageService;
		private readonly UserManager<DbUser> userManager;
		private readonly IJobService jobService;
		private readonly ILogger<InventoryController> logger;

		public InventoryController(IInventoryService inventoryService, IDepartmentService departmentService,
								   IStorageService storageService, UserManager<DbUser> userManager,
								   IJobService jobService, ILogger<InventoryController> logger)
		{
			this.inventoryService = inventoryService;
			this.departmentService = departmentService;
			this.storageService = storageService;
			this.userManager = userManager;
			this.jobService = jobService;
			this.logger = logger;
		}

		[HttpGet]
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(HttpContext.User);
			var departments = await departmentService.GetDepartments();
			var vm = new InventoryViewModel();
			vm.CurrentJob = user.CurrentJobId == null ? null : await jobService.GetJob(user.CurrentJobId.Value);
			vm.AddInventoryItemViewModel = new AddInventoryItemViewModel()
			{
				DepartmentList = departments.Select(d => new SelectListItem
				{
					Value = d.DepartmentId.ToString(),
					Text = d.DepartmentName
				}).ToList(),
				BrandList = new List<SelectListItem>
				{
					new SelectListItem{Value = "A", Text="苹果产品"},
					new SelectListItem{Value = "R", Text = "非苹果产品"}
				},
				Brand = "A"
			};
			vm.LastAddedInventoryItem = user.CurrentJobId == null ? null :
				await inventoryService.GetLastInventoryItemOfUser(user.CurrentJobId.Value, user.UserId);

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
						ProductId = viewModel.ProductId ?? string.Empty,
						Quantity = viewModel.Quantity <= 0 ? 1 : viewModel.Quantity,
						ImageFiles = new List<string>(),
						DepartmentId = int.Parse(viewModel.DepartmentId),
						Brand = viewModel.Brand,
						Model = viewModel.Model,
						SerialNumber = viewModel.SerialNumber
					};

					//upload files
					if (viewModel.Images?.Any(f => f.Length > 0) == true)
					{
						foreach (var file in viewModel.Images.Where(f => f.Length > 0))
						{
							var fileName = $"{user.CurrentJobId}-{user.UserId}-{Guid.NewGuid():N}.jpg";
							using (var readStream = file.OpenReadStream())
							{
								await storageService.UploadStreamToFile(fileName, readStream);
							}
							item.ImageFiles.Add(fileName);
						}
					}

					//save data to database
					item = await inventoryService.AddInventoryItem(item);

					logger.LogInformation($"User {user.UserName}({user.DisplayName}) add new item to job {item.JobId} with {item.ImageFiles.Count} pictures");
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
