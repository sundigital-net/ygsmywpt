﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    /// <summary>
    /// 登录页面，账号、密码、记住我
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string Account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Display(Name = "保持登录")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
