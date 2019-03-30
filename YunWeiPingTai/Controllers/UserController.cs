using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YunWeiPingTai.Common;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userSvc;
        //private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUserService userSvc)
        {
            _userSvc = userSvc;
        }

        public IActionResult Index()
        {
            return View();
        }
         public IActionResult List()
         {
             return View();
         }
        public string LoadData()
        {
            var users = _userSvc.GetAll();
            var tableData = new TableDataModel()
            {
                count = users.Length,
                data=users
            };
            return JsonConvert.SerializeObject(tableData, new IsoDateTimeConverter() {DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }
        
        [HttpPost]
        public IActionResult Delete(long id)
        {
            _userSvc.MarkDeleted(id);
            return Json(new AjaxResult {Status = "ok"});
        }
        public IActionResult Audit(bool status, long id)
        {
            _userSvc.UpdateStatus(id,status);
            return Json(new AjaxResult() {Status = "ok", Data = status ? "已通过" : "已驳回"});
        }
    }
}