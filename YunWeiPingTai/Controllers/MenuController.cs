using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YunWeiPingTai.Common;
using YunWeiPingTai.DTO;
using YunWeiPingTai.DTO.RequestModel;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuController> _logger;

        public MenuController(IMenuService menuService, ILogger<MenuController> logger)
        {
            _logger = logger;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            var menus = _menuService.GetAll(0);
            return View(menus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm]MenuAddEditPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }

            var dto = new MenuDTO()
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                Name = model.Name,
                IconUrl = model.IconUrl,
                LinkUrl = model.LinkUrl,
                ParentId = model.ParentId,
                Permission = model.Permission,
                Sort = model.Sort
            };
            var id= _menuService.AddOrEdit(dto);
            if (id <= 0)
            {
                return Json(new AjaxResult() { Status = "error",ErrorMsg = "已存在相同的菜单名称"});
            }
            return Json(new AjaxResult() {Status = "ok"});
        }
        public IActionResult List()
        {
            return View();
        }

        public string LoadData([FromQuery]MenuRequestModel model)
        {
            var tableData = _menuService.LoadData(model);
            return JsonHelper.ObjectToJSON(tableData);
        }
        [HttpGet]
        public string LoadDataWithParentId([FromQuery]int parentId = -1)
        {
            return JsonHelper.ObjectToJSON(_menuService.GetChildListByParentId(parentId));
        }
        [HttpGet]
        public bool IsExistsName([FromQuery]MenuAddEditPostModel item)
        {
            var result = _menuService.IsExistsName(item.Name,item.Id);
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long[] menuId)
        {
            _menuService.MarkDelete(menuId);
            return Json(new AjaxResult(){Status = "ok"});
        }
    }
}