using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Web;
using YunWeiPingTai.Common;

namespace YunWeiPingTai.Configuration
{
    /// <summary>
    /// 跳过属性检查
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple =false,Inherited =true)]
    public sealed class SkipUserAuthorizeAttribute:Attribute,IFilterMetadata
    {

    }
    /// <summary>
    /// 用户登录验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public const string UserAuthenticationScheme = "UserAuthenticationScheme";//自定义一个默认的登录方案
        public UserAuthorizeAttribute()
        {
            this.AuthenticationSchemes = UserAuthenticationScheme;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //获取登录方案
            var authenticate = context.HttpContext.AuthenticateAsync(UserAuthorizeAttribute.UserAuthenticationScheme);
            //if (authenticate.Result.Succeeded || this.SkipUserAuthorize(context.ActionDescriptor))
            if (authenticate.Result.Succeeded)
            {
                return;
            }

            HttpRequest httpRequest = context.HttpContext.Request;
            if(httpRequest.IsAjaxRequest())//ajax请求
            {
                AjaxResult result = new AjaxResult();
                result.Status = "redirect";//需要重定向
                result.ErrorMsg = "登录超时";
                result.Data = "~/Account/Login";
                context.Result = new JsonResult(result);
            }
            else
            {
                RedirectResult redirectResult = new RedirectResult("~/Account/Login");
                context.Result = redirectResult;
            }
            
            return;
        }
        protected virtual bool SkipUserAuthorize(ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.FilterDescriptors
                .Where(a => a.Filter is SkipUserAuthorizeAttribute).Any();
        }
    }
}
