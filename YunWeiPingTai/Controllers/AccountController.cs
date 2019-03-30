using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YunWeiPingTai.Common;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;
using YunWeiPingTai.Service;
using YunWeiPingTai.Services;

namespace YunWeiPingTai.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userSvc;
        private readonly ILogger<AccountController> _logger;
        private readonly IOptions<EmailSettings> _options;
        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<IdentityUser> _userManager;

        public AccountController(
            IUserService userSvc, IEmailSender emailSender,
            ILogger<AccountController> logger,
            IOptions<EmailSettings> options)
        {
            _userSvc = userSvc;
            _logger = logger;
            _options = options;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            /*
            //获取cookie中用户信息
            var adminAccount =HttpContext.User.Claims.SingleOrDefault(t=>t.Type=="Account");
            string account = adminAccount == null ? "" : adminAccount.Value;
            var adminPwd = HttpContext.User.Claims.SingleOrDefault(t => t.Type == "Password");
            string pwd = adminPwd == null ? "" : adminPwd.Value;
            var rememberMe = HttpContext.User.Claims.SingleOrDefault(t => t.Type == "RememberMe");
            string rem = rememberMe == null ? "" : rememberMe.Value;
            LoginViewModel model = new LoginViewModel();
            //如果选择了记住我，则传递账号和密码
            model.ReturnUrl = returnUrl;
            model.RememberMe = rem == "on";
            model.Account = model.RememberMe?account:"";
            model.Password = model.RememberMe? pwd:"";
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);*/
            return View();
        }

        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> LoginAsync(LoginPostModel model)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }

            #region 验证码
            var serverCaptcha = (string)TempData["CaptchaStr"];
            if (string.IsNullOrEmpty(serverCaptcha) || serverCaptcha.ToLower() != model.Captcha.ToLower())//不区分大小写，一律转换成小写再去比较
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误。" });
            }
            #endregion
            //账号密码的验证


            var ip = HttpContext.GetClientUserIp();
            
            //账号密码
            var user = _userSvc.Login(model.Account, model.Password,ip);
            if (user==null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "账号或密码错误。" });
            }
            
            //账号状态
            if (user.IsLock)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "该账号未审核或已被锁定" });
            }
            
            var claims=new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Name),//姓名
                new Claim(ClaimTypes.MobilePhone,user.PhoneNum),//手机号
                new Claim(ClaimTypes.Email,user.Email),//邮箱
                new Claim(ClaimTypes.Role,user.RoleId.ToString()),//角色
                new Claim("Id",user.Id.ToString()),
                new Claim("SigninCount",user.SigninCount.ToString()),
                new Claim("LastSigninTime",user.LastSigninTimeStr),
                new Claim("LastSigninIP",user.LastSigninIP)
            };
            var claimsIdentity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            
            return Json(new AjaxResult { Status = "ok" });

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }
            //验证码
            var serverCaptcha = (string)TempData["CaptchaStr"];
            if (string.IsNullOrEmpty(serverCaptcha) || serverCaptcha.ToLower() != model.Captcha.ToLower())//不区分大小写，一律转换成小写再去比较
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误。" });
            }

            //
            var id = _userSvc.AddNew(model.PhoneNum, model.Email, model.Name, model.Password);
            if (id == -1)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "电子邮箱或者手机号已经存在。" });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        #region 账号唯一性验证，暂不用
        [HttpPost]
        public IActionResult Validate(string account)
        {
            var admin = _userSvc.GetUser(account);
            if (admin == null)
                return Content("true");
            else
                return Content("false");
        }
        #endregion

        public IActionResult GetCaptcha()
        {
            try
            {
                //创建字符串并保存到TempData
                var code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.AbcVerifyCode, 4);
                TempData["CaptchaStr"] = code;
                //生成img
                var codeImg = VerifyCodeHelper.GetSingleObj().CreateByteByImgVerifyCode(code, 90, 36);
                return File(codeImg, @"image/jpeg");
            }
            catch (Exception ex)
            {
                _logger.LogError("生成验证码出现错误", ex);
                return File("/img/no-picture.png", @"image/jpeg");
            }
        }

        [HttpGet]
        [Route("Account/Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.Clear();
            return RedirectToAction(nameof(AccountController.Login));
        }

        [HttpGet]
        public IActionResult ForgetPwdStepOne()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPwdStepOne(PwdForgetStepOnePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }
            //验证码
            var serverCaptcha = (string)TempData["CaptchaStr"];
            if (string.IsNullOrEmpty(serverCaptcha) || serverCaptcha.ToLower() != model.Captcha.ToLower())//不区分大小写，一律转换成小写再去比较
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误。" });
            }
            //账户号是否存在
            var user = _userSvc.GetUser(model.Account);
            if (user == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "账号不存在。" });
            }

            //发送验证码
            try
            {
                //生成验证码
                var code = VerifyCodeHelper.GetSingleObj()
                    .CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.NumberVerifyCode, 6);
                TempData["PwdForgetCaptcha"] = code;
                TempData["Account"] = model.Account;
                await _emailSender.SendEmailAsync(model.Account, EmailType.Captcha, $"验证码：{code}，请在5分钟内验证。如非本人操作，请忽略。");
                //sender.SendEmail(model.Account, EmailType.Captcha, $"验证码：{code}，请在5分钟内验证。如非本人操作，请忽略。");
                _logger.LogInformation("邮件发送：To" + model.Account + "，类型：" + EmailType.Captcha);
            }
            catch (Exception e)
            {
                _logger.LogError("邮箱验证码发送失败", e);
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱验证码发送失败" });
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public IActionResult ForgetPwdStepTwo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPwdStepTwo(string emailCaptcha)
        {
            if (string.IsNullOrEmpty(emailCaptcha))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱验证码必填" });
            }
            //验证
            var serverCaptha = (string)TempData["PwdForgetCaptcha"];
            if (string.IsNullOrEmpty(serverCaptha) || serverCaptha.Trim() != emailCaptcha.Trim())
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "邮箱验证码错误，请重新发送" });
            }

            return Json(new AjaxResult() { Status = "ok" });
        }

        [HttpGet]
        public IActionResult ForgetPwdStepThree()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPwdStepThree(PwdForgetStepThreePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }

            var account = (string)TempData["Account"];
            if (string.IsNullOrEmpty(account))
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "账号信息查找失败" });
            }
            _userSvc.ChangePwd(account, model.Password);
            return Json(new AjaxResult() { Status = "ok" });
        }

        
    }
}