using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using StocktakingOnline.Web.Models.Database;
using StocktakingOnline.Web.Models.ViewModel;
using Microsoft.Extensions.Configuration;

namespace StocktakingOnline.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly UserManager<DbUser> userManager;
		private readonly IConfiguration configuration;
		private readonly SignInManager<DbUser> signInManager;
		private readonly ILogger<AccountController> logger;

		public AccountController(UserManager<DbUser> userManager, IConfiguration configuration,
								 SignInManager<DbUser> signInManager, ILogger<AccountController> logger)
		{
			this.userManager = userManager;
			this.configuration = configuration;
			this.signInManager = signInManager;
			this.logger = logger;
		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					logger.LogInformation($"User {model.UserName} logged in.");
					return RedirectToLocal(returnUrl);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(model);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			logger.LogInformation($"Register userName={model.UserName} displayName={model.DisplayName}");
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				var registerKey = configuration["RegisterKey"] ?? string.Empty;
				if (registerKey == (model.RegisterKey ?? string.Empty))
				{
					var user = new DbUser
					{
						UserName = model.UserName,
						DisplayName = model.DisplayName,
						CreatedTime = DateTime.UtcNow
					};
					var result = await userManager.CreateAsync(user, model.Password);
					if (result.Succeeded)
					{
						logger.LogInformation($"User created a new account {user.UserName} with password.");
						//await signInManager.SignInAsync(user, isPersistent: false);
						return RedirectToLocal(returnUrl);
					}
					else
					{
						AddErrors(result);
						foreach (var err in result.Errors)
						{
							logger.LogWarning($"Register error code={err.Code} description={err.Description}");
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "管理员验证码错误");
				}
			}
			else
			{
				logger.LogWarning("Register model is not valid");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			logger.LogInformation("User logged out.");
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		#region Helpers

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}

		#endregion
	}
}
