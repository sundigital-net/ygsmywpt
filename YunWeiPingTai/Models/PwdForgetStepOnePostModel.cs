using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class PwdForgetStepOnePostModel
    {
        [Display(Name = "验证码")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string Captcha { get; set; }
        [Display(Name = "账号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [EmailAddress(ErrorMessage = "{0}不是正确的电子邮箱格式")]
        public  string Account { get; set; }
    }
}
