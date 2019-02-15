using Microsoft.Extensions.Logging;
using MyWebApi.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Service
{
    public class LocalMailService:IMailService
    {
        //private string _mailTo = "developer@qq.com";
        //private string _mailFrom = "noreply@alibaba.com";
        private readonly string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private readonly string _mailFrom =Startup.Configuration["mailSettings:mailFromAddress"];
        private readonly ILogger<CloudMailService> _logger;

        public LocalMailService(ILogger<CloudMailService> logger)
        {
            _logger = logger;
        }
        public void Send(string subject, string msg)
        {
            _logger.LogInformation($"从{_mailFrom}给{_mailTo}通过{nameof(LocalMailService)}发送了邮件，Debug操作："+subject+"，信息："+msg);
        }
    }
}
