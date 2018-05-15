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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
