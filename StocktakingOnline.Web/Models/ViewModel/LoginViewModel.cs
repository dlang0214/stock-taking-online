using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace StocktakingOnline.Web.Models.ViewModel
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "登录名为必填项")]
		[Display(Name = "登录名")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "密码为必填项")]
		[DataType(DataType.Password)]
		[Display(Name = "密码")]
		public string Password { get; set; }

		[Display(Name = "记住我的登录状态")]
		public bool RememberMe { get; set; }
	}
}
