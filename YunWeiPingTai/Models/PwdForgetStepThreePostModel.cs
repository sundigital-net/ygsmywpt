using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class PwdForgetStepThreePostModel
    {
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0}必填.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度为{2}-{1}之间.")]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0}必填.")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致.")]
        public string PasswordConfirm { get; set; }
    }
}
