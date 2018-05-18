using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StocktakingOnline.Web.Services.Declaration;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.Database;
using Microsoft.Extensions.Logging;
using StocktakingOnline.Web.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using StocktakingOnline.Web.Models.Domain;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	[Route("Preview")]
	public class PreviewController : Controller
	{
		private readonly IInventoryService inventoryService;
		private readonly IDbService dbService;
		private readonly IDepartmentService departmentService;
		private readonly IJobService jobService;
		private readonly UserManager<DbUser> userManager;
		private readonly ILogger<PreviewController> logger;

		public PreviewController(IInventoryService inventoryService, IDbService dbService,
								 IDepartmentService departmentService,
								 IJobService jobService, UserManager<DbUser> userManager, ILogger<PreviewController> logger)
		{
			this.inventoryService = inventoryService;
			this.dbService = dbService;
			this.departmentService = departmentService;
			this.jobService = jobService;
			this.userManager = userManager;
			this.logger = logger;
		}

		// GET: /<controller>/
		[Route("Index")]
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

		[HttpPost]
		[Route("Items/{recordId}")]
		public async Task Edit(string recordId, EditInventoryItemViewModel viewModel)
		{
			if (string.IsNullOrWhiteSpace(recordId) || !ModelState.IsValid)
			{
				return;
			}
			var user = await userManager.GetUserAsync(HttpContext.User);
			var item = await inventoryService.GetInventoryItemByRecordId(recordId);
			//必须是当前Job 暂时不验证是否是当前用户录入的(有可能是Supervisor修改,将来有UserRole系统之后再来判断)
			if (user == null || user.CurrentJobId.GetValueOrDefault() != item.JobId) //user.UserId != item.UserId
			{
				return;
			}
			item.ProductId = viewModel.ProductId;
			item.Quantity = viewModel.Quantity;
			item.DepartmentId = viewModel.DepartmentId;
			item.Brand = viewModel.Brand;
			item.Model = viewModel.Model;
			item.SerialNumber = viewModel.SerialNumber;
			await inventoryService.EditInventoryItemWithoutPicture(recordId, item);
		}

		[HttpGet]
		[Route("Departments")]
		public async Task<List<Department>> GetDepartmentList()
		{
			var departments = await departmentService.GetDepartments();
			return departments;
		}

		[HttpGet]
		[Route("Items/{recordId}")]
		public async Task<InventoryItem> GetInventoryItem(string recordId)
		{
			if (string.IsNullOrWhiteSpace(recordId))
			{
				return null;
			}
			return await inventoryService.GetInventoryItemByRecordId(recordId);
		}

		[HttpDelete]
		[Route("Items/{recordId}")]
		public async Task DeleteInventoryItem(string recordId)
		{
			if (string.IsNullOrWhiteSpace(recordId))
			{
				return;
			}
			var item = await inventoryService.GetInventoryItemByRecordId(recordId);
			var user = await userManager.GetUserAsync(HttpContext.User);
			//必须处于当前job且是当前用户录入的数据才可以删除
			if (item == null || user == null || item.JobId != user.CurrentJobId.GetValueOrDefault()) //item.UserId != user.UserId
			{
				await inventoryService.DeleteInventoryItem(recordId);
			}
		}
	}
}
