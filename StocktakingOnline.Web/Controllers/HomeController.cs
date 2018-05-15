using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StocktakingOnline.Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace StocktakingOnline.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> logger;
		private readonly IConfiguration configuration;

		public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
		{
			this.logger = logger;
			this.configuration = configuration;
		}

		public IActionResult Index()
		{
			logger.LogDebug("Display Index page");
			Debug.WriteLine(configuration.GetConnectionString("DefaultConnection"));
			return View();
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
