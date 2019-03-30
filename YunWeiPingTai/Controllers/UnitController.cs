using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YunWeiPingTai.Common;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;
using YunWeiPingTai.Validation;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class UnitController : Controller
    {
        private readonly IUnitService _unitSvc;
        private readonly ILogger<UnitController> _logger;

        public UnitController(IUnitService unitSvc,ILogger<UnitController> logger)
        {
            _unitSvc = unitSvc;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm]UnitAddEditPostModel model)
        {
        //    var validationRules = new UnitValidation();
        //    var result = validationRules.Validate(model);
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() {Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState)});
            }

            var id=  _unitSvc.AddOrEdit(model.Id,model.Name, model.Address, model.LinkMan, model.Tel, model.PhoneNum);
            if (id <= 0)
            {
                return Json(new AjaxResult() { Status = "error",ErrorMsg = "已存在相同的单位名称"});
            }
            return Json(new AjaxResult() {Status = "ok"});
        }

        public IActionResult List()
        {

            return View();
        }

        public string LoadData()
        {
            var users = _unitSvc.GetAll();
            var tableData = new TableDataModel()
            {
                count = users.Count,
                data = users
            };
            return JsonHelper.ObjectToJSON(tableData);
        }
    }
}