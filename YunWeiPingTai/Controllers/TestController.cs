using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using YunWeiPingTai.Common;
using YunWeiPingTai.Models;
using YunWeiPingTai.Services;

namespace YunWeiPingTai.Controllers
{
    public class TestController : Controller
    {
        private readonly IOptions<EmailSettings> _emailOptions;
        private readonly ILogger<TestController> _logger;
        private readonly IEmailSender _emailSender;

        public TestController(IOptions<EmailSettings> emailOptions,ILogger<TestController> logger,IEmailSender emailSender)
        {
            _emailOptions = emailOptions;
            _logger = logger;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                string code = "259878";
                await _emailSender.SendEmailAsync("364572026@qq.com",EmailType.Captcha ,$"验证码：{code}，如非本人操作请忽略。");
                _logger.LogInformation("发送邮件，to："+"364572026@qq.com"+",类型："+EmailType.Captcha);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("出错了："+e.Message);
            }
            
        }

        public IActionResult Send()
        {
            IRestResponse response=  _emailSender.SendEmail();
            if(response.IsSuccessful)
                return Content("ok");
            else
            {
                return Content("error");
            }
        }
    }
}