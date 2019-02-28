using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class LoginPostModel
    {
        [Display(Name = "账号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [EmailAddress(ErrorMessage = "请填写正确格式的电子邮箱.")]
        public string Email { get; set; }
        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}是必填项")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}长度应大于{2}并且小于{1}")]
        public string Password { get; set; }
        [Display(Name = "验证码")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(4, ErrorMessage = "{0}长度应是{1}")]//{0}表示Display的Name属性, {1}表示当前注解的第一个变量, {2}表示当前注解的第二个变量.
        public string Captcha { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
