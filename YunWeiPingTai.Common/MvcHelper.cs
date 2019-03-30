using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace YunWeiPingTai.Common
{
    public static class MvcHelper
    {
        //yuyuyuyu
        public static string GetValidMsg(ModelStateDictionary modelState)
        {
            StringBuilder sb = new StringBuilder();
            foreach( var key in modelState.Keys)
            {
                if(modelState[key].Errors.Count<=0)
                {
                    continue;
                }
                sb.Append("属性【").Append(key).Append("】错误：");
                foreach(var error in modelState[key].Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            bool result = false;
            var xreq = request.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = request.Headers["x-requested-with"] == "XMLHttpRequest";
            }

            return result;
        }
        public static string GetUserName(this HttpContext context)
        {
            return context.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.Name).Value;
        }
    }
}
