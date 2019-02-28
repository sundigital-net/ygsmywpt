using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    [UserAuthorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userSvc;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IUserService userSvc,ILogger<HomeController> logger)
        {
            _userSvc = userSvc;
            _logger = logger;
        }
        public IActionResult Index()
        {
            //long id= _userSvc.AddNew("15376261308", "364572026@qq.com", "张汉英", "123456");
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
            return View();

        }

        public IActionResult Welcome()
        {
            return View();
        }
        
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
