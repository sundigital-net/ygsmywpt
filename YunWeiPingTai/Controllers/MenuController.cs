using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YunWeiPingTai.Common;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
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
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public string LoadData()
        {
            var menus = _menuService.GetAll();
            var tableData = new TableDataModel()
            {
                count = menus.Length,
                data = menus
            };
            return JsonHelper.ObjectToJSON(tableData);
        }
        [HttpGet]
        public string LoadDataWithParentId([FromQuery]int parentId = -1)
        {
            return JsonHelper.ObjectToJSON(_menuService.GetChildListByParentId(parentId));
        }
    }
}