using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StocktakingOnline.Web.Models.ViewModel;
using StocktakingOnline.Web.Services.Declaration;

namespace StocktakingOnline.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IJobService jobService;
		private readonly ILogger<HomeController> logger;

		public HomeController(IJobService jobService, ILogger<HomeController> logger)
		{
			this.jobService = jobService;
			this.logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var jobs = await jobService.GetJobs();
			var vm = new HomeViewModel
			{
				Jobs = jobs
			};
			return View(vm);
		}

		public async Task<IActionResult> SwitchJob(int jobId)
		{
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
