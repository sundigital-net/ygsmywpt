using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YunWeiPingTai.Common;
using YunWeiPingTai.DTO.RequestModel;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public IActionResult List()
        {
            return View();
        }

        public string LoadData([FromQuery]DeviceRequestModel model)
        {
            var tableData = _deviceService.LoadData(model);
            
            return JsonHelper.ObjectToJSON(tableData);

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm]DeviceAddEditPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() {Status = "error", ErrorMsg = MvcHelper.GetValidMsg(ModelState)});
            }

            var id= _deviceService.AddOrEdit(model.Id, model.Name, model.Version, model.Maker);
            if (id <= 0)
            {
                return Json(new AjaxResult() { Status = "error" ,ErrorMsg = "已存在相同名称和型号的设备"});
            }
            return Json(new AjaxResult() {Status = "ok"});
        }

        public bool IsExistsName([FromQuery] string name,string version,long id)
        {
            var result = _deviceService.IsExistsName(name,version, id);
            return result;
        }
    }
}