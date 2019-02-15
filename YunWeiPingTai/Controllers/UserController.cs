using System.Linq;
using Microsoft.AspNetCore.Mvc;
using YunWeiPingTai.Common;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.IService;

namespace YunWeiPingTai.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userSvc;

        public UserController(IUserService userSvc)
        {
            _userSvc = userSvc;
        }
       
        public IActionResult NewList()
        {
            var users = _userSvc.GetAll().Where(t => t.Status == ShenHeZhuangTai.ShenQing).ToArray();
            return View(users);
        }
        public IActionResult List()
        {
            var users = _userSvc.GetAll();
            return  View(users);
        }
        
        [HttpPost]
        public IActionResult Delete(long id)
        {
            _userSvc.MarkDeleted(id);
            return Json(new AjaxResult {Status = "ok"});
        }
        public IActionResult Audit(bool status, long id)
        {
            var sta = status ? ShenHeZhuangTai.TongGuo : ShenHeZhuangTai.BoHui;
            _userSvc.UpdateStatus(id,sta);
            return Json(new AjaxResult() {Status = "ok", Data = status ? "已通过" : "已驳回"});
        }
    }
}