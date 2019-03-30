using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.Extensions;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;
using YunWeiPingTai.Common;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IUserService userSvc,ILogger<HomeController> logger,IRoleService roleService)
        {
            _userSvc = userSvc;
            _logger = logger;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            //long id = _userSvc.AddNew("15376261308", "364572026@qq.com", "张汉英", "123456");
            //return Content(id.ToString());
            //string userId = HttpContext.Session.GetString("AdminLoginId");
            //try
            //{
            //    long id = Convert.ToInt64(userId);
            //    var user = _userSvc.GetUser(id);
            //    return View(user);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("获取用户信息出错",ex);
            //    return Content("出错了："+ex.Message);
            //}


            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return View();
            //}
            ViewData["Name"] = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            return View();
            //return Content("没有授权");

        }

        public string GetMenu()
        {
            var roleId = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Role)?.Value;
            var menuViewTree = _roleService.GetMenusByRoleId(Convert.ToInt32(roleId))
                .GenerateTree(x => x.Id, x => x.ParentId);
            return JsonHelper.ObjectToJSON(menuViewTree);
        }

        public IActionResult Welcome()
        {
            var userCount = _userSvc.GetAll().Count();
            ViewData["UserCount"]=userCount.ToString();
            ViewData["UserName"] = User.Claims.FirstOrDefault(t=>t.Type==ClaimTypes.Name)?.Value;
            ViewData["SigninCount"] = User.Claims.FirstOrDefault(t=>t.Type== "SigninCount")?.Value;
            ViewData["LastSigninTime"] = User.Claims.FirstOrDefault(t=>t.Type== "LastSigninTime")?.Value;
            ViewData["LastSigninIP"] = User.Claims.FirstOrDefault(t => t.Type == "LastSigninIP")?.Value;
            return View();
        }

        [HttpGet]
        public IActionResult ChangePwd()
        {
            //修改了一下 Hany
            ViewData["Id"] = User.Claims.FirstOrDefault(t => t.Type == "Id")?.Value;
            ViewData["UserName"] = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Name)?.Value;
            return View();
        }

        //[HttpPost]
        //public IActionResult ChangePwd()
        //{

        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
