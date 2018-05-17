using System;
using System.ComponentModel.DataAnnotations;

namespace StocktakingOnline.Web.Models.ViewModel
{
	public class RegisterViewModel
    {
        [Required]
        [Display(Name = "登录名")]
        public string UserName { get; set; }

		[Required]
		[Display(Name = "真实姓名")]
		public string DisplayName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}长度须位于 {2} 位与 {1} 位之间.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

		[Display(Name ="管理员验证码")]
		public string RegisterKey { get; set; }
	}
}
