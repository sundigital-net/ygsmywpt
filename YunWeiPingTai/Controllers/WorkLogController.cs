using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using YunWeiPingTai.Common;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Controllers
{
    [Authorize]
    public class WorkLogController : Controller
    {
        public IWorkLogService _logSvc;
        public IUserService _userSvc;
        public WorkLogController(IWorkLogService logService,IUserService userService)
        {
            _logSvc = logService;
            _userSvc = userService;
        }
        [UserAuthorize]
        [HttpGet]
        public IActionResult List()
        {
            var serverClaimId = HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
            string userId = serverClaimId == null ? "" : serverClaimId.Value;
            var user = _userSvc.GetUser(Convert.ToInt64(userId));
            List<WorkLogDTO> list = new List<WorkLogDTO>();
            if (user.Role == UserRoles.Engineer)
            {
                list = _logSvc.GetLogs(user.Id).ToList();
            }
            else
            {
                list= _logSvc.GetLogs().ToList();
            }
            
            var logs = list.ToArray();//日志
            var replies = _logSvc.GetReplies();
            WorkLogListViewModel model = new WorkLogListViewModel()
            {
                WorkLogs =logs,
                WorkLogReplies = replies,
            };
            return View(model);
        }

        
        [UserAuthorize]
        [HttpPost]
        public IActionResult Add(string log)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
            string id = userId == null ? "" : userId.Value;
            if (string.IsNullOrEmpty(log))
            {
                return Json(new AjaxResult {Status = "error", ErrorMsg = "请填写日志"});
            }

            long logId= _logSvc.AddNewLog(Convert.ToInt64(id), log);
            if (logId < 0)
            {
                return Json(new AjaxResult() {Status = "error", ErrorMsg = "今天您已上报日志。"});
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [UserAuthorize]
        [HttpPost]
        public IActionResult Delete(long id)
        {
            _logSvc.DeleteLog(id);
            return Json(new AjaxResult() {Status = "ok"});
        }

        [UserAuthorize]
        [HttpPost]
        public IActionResult AddReply(long logId, string reply)
        {
            if (logId <= 0 || string.IsNullOrEmpty(reply))
            {
                return Json(new AjaxResult() {Status = "error", ErrorMsg = "评论内容缺失"});
            }
            var userId = HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
            string id = userId == null ? "" : userId.Value;
            _logSvc.AddNewReply(logId, reply, Convert.ToInt64(id));
            return Json(new AjaxResult() {Status = "ok"});
        }
    }
}