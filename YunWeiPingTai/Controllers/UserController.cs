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
using YunWeiPingTai.DTO.RequestModel;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleService;
        public UserController(IUserService userSvc,IRoleService roleService)
        {
            _userSvc = userSvc;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var roles = _roleService.GetAll();
            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm]UserAddEditPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }

            if (model.Id <= 0)//新增
            {
                var pwd = model.PhoneNum.Substring(5);
                var id = _userSvc.AddNew(model.PhoneNum, model.Email, model.UserName,pwd, model.RoleId, model.IsLock);
                if (id <= 0)
                {
                    return Json(new AjaxResult() { Status = "error",ErrorMsg = "已存在相同的手机号或者邮箱"});
                }

                return Json(new AjaxResult() {Status = "ok"});
            }
            else//修改
            {
                var id= _userSvc.Update(model.Id, model.PhoneNum, model.Email, model.UserName, model.RoleId);
                if (id <= 0)
                {
                    return Json(new AjaxResult() { Status = "error", ErrorMsg = "已存在相同的手机号或者邮箱" });
                }

                return Json(new AjaxResult() { Status = "ok" });
            }
            
        }
         public IActionResult List()
         {
             return View();
         }
        public string LoadData([FromQuery]UserRequestModel model)
        {
            var users = _userSvc.LoadData(model);

            return JsonHelper.ObjectToJSON(users);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long[] userId)
        {
            _userSvc.MarkDeleted(userId);
            return Json(new AjaxResult {Status = "ok"});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeLockStatus(long id,bool status)
        {
            _userSvc.UpdateStatus(id,status);
            return Json(new AjaxResult() { Status = "ok"});
        }

        public bool IsExistsAccount([FromQuery] string account, long id)
        {
            var result = _userSvc.IsExistsAccount(account, id);
            return result;
        }


    }
}