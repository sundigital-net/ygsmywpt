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
using YunWeiPingTai.DTO.RequestModel;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;
using YunWeiPingTai.Validation;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class UnitController : Controller
    {
        private readonly IUnitService _unitSvc;
        private readonly IDeviceService _deviceSvc;
        private readonly ILogger<UnitController> _logger;

        public UnitController(IUnitService unitSvc,ILogger<UnitController> logger,IDeviceService deviceSvc)
        {
            _unitSvc = unitSvc;
            _logger = logger;
            _deviceSvc = deviceSvc;
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

        public string LoadData([FromQuery]UnitRequestModel model)
        {
            var tableData = _unitSvc.LoadData(model);
            return JsonHelper.ObjectToJSON(tableData);
        }

        public bool IsExistsName([FromQuery] string name, long id)
        {
            var result = _unitSvc.IsExistsName(name, id);
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long[] ids)
        {
            _unitSvc.UnitDel(ids);
            return Json(new AjaxResult() {Status = "ok"});
        }

        [HttpGet]
        public IActionResult DeviceList(long unitId)
        {
            return View(unitId);
        }

        public string DeviceLoadData([FromQuery]UnitDeviceRequestModel model)
        {
            var tableData = _unitSvc.LoadData(model);
            return JsonHelper.ObjectToJSON(tableData);
        }

        [HttpGet]
        public IActionResult DeviceIndex()
        {
            var devices = _deviceSvc.GetAll();
            return View(devices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeviceIndex([FromForm]UnitDeviceAddEditPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState) });
            }
            var id = User.Claims.FirstOrDefault(t => t.Type == "Id")?.Value;
            var unitDeviceId =_unitSvc.AddOrEdit(model.Id, model.UnitId, model.DeviceId, Convert.ToInt64(id), model.SNCode);
            if (unitDeviceId <= 0)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "已存在相同的SN码" });
            }

            return Json(new AjaxResult() {Status = "ok"});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDevice(long[] ids)
        {
            _unitSvc.UnitDeviceDel(ids);
            return Json(new AjaxResult() {Status = "ok"});
        }
    }
}