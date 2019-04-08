using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class RegisterPostModel
    {
        [Required(ErrorMessage ="{0}必填.")]
        [Display(Name="姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage ="{0}必填.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="请填写正确格式的电子邮箱.")]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Display(Name="手机号")]
        [Required(ErrorMessage ="{0}必填.")]
        [StringLength(11,MinimumLength =11,ErrorMessage ="手机号码长度为{1}.")]
        public string PhoneNum { get; set; }

        [Display(Name="密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0}必填.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度为{2}-{1}之间.")]
        public string Password { get; set; }

        [Display(Name="确认密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0}必填.")]
        [Compare(nameof(Password),ErrorMessage ="两次输入的密码不一致.")]
        public string PasswordConfirm { get; set; }

        [Display(Name="验证码")]
        [Required(ErrorMessage = "{0}必填.")]
        public string Captcha { get; set; }
        
        [Display(Name ="角色类型")]
        [Required(ErrorMessage ="{0}必填.")]
        public long RoleId { get; set; }
    }
}
