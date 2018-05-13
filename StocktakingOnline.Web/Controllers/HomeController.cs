using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StocktakingOnline.Web.Models.ViewModel;
using StocktakingOnline.Web.Services.Declaration;
using Microsoft.AspNetCore.Identity;
using StocktakingOnline.Web.Models.Database;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IJobService jobService;
		private readonly IUserService userService;
		private readonly ILogger<HomeController> logger;
		private readonly UserManager<DbUser> userManager;

		public HomeController(IJobService jobService, IUserService userService, ILogger<HomeController> logger, UserManager<DbUser> userManager)
		{
			this.jobService = jobService;
			this.userService = userService;
			this.logger = logger;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var jobs = await jobService.GetJobs();
			var user = await userManager.GetUserAsync(HttpContext.User);
			var vm = new HomeViewModel
			{
				Jobs = jobs,
				UserDisplayName = user.DisplayName,
				UserCurrentJobId = user.CurrentJobId.GetValueOrDefault()
			};
			return View(vm);
		}

		public async Task<IActionResult> SwitchJob(int jobId)
		{
			var user = await userManager.GetUserAsync(HttpContext.User);
			await userService.SwitchToJob(user, jobId);
			return new RedirectToActionResult("Index", "Home", null);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
