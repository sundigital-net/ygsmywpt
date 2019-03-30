using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YunWeiPingTai.Common;
using YunWeiPingTai.Extensions;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService,ILogger<RoleController> logger,IMenuService menuService)
        {
            _roleService = roleService;
            _logger = logger;
            _menuService = menuService;
        }
        [HttpGet]
        public IActionResult Index(long id)
        {
            if (id > 0)
            {
                ViewData["MenuIds"] = _roleService.GetIdsByRoleId(id).ArrayToString();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm]RoleAddEditPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error",ErrorMsg = MvcHelper.GetValidMsg(ModelState)});
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    var roleId = _roleService.AddOrEdit(model.Id, model.Name, model.Remark);
                    if (roleId <= 0)
                    {
                        return Json(new AjaxResult() { Status = "error", ErrorMsg = "出错了" });
                    }

                    _menuService.AddMenuIds(roleId, model.MenuIds);
                    tran.Complete();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "出错了，详见错误日志" });
            }
            
            
            return Json(new AjaxResult() {Status = "ok"});
        }

        public IActionResult List()
        {
            return View();
        }

        public string LoadData()
        {
            var roles = _roleService.GetAll();
            var tableData = new TableDataModel()
            {
                count = roles.Length,
                data = roles
            };
            return JsonHelper.ObjectToJSON(tableData);
        }
    }
}