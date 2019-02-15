using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YunWeiPingTai.Common;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    public class UnitController : Controller
    {
        private readonly IUnitService _unitSvc;

        public UnitController(IUnitService unitSvc)
        {
            _unitSvc = unitSvc;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(UnitAddPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error",ErrorMsg = MvcHelper.GetValidMsg(ModelState)});
            }
            return Json(new AjaxResult() {Status = "ok"});
        }
        [HttpGet]
        public IActionResult Edit(long id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit()
        {
            return Json(new AjaxResult {Status = "ok"});
        }

        public IActionResult List()
        {

            return View();
        }
    }
}