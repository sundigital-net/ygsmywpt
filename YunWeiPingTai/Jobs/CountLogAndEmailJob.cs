using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service;
using YunWeiPingTai.Services;

namespace YunWeiPingTai.Jobs
{
    public class CountLogAndEmailJob:Job
    {
        private readonly IUserService _userSvc;
        private readonly IWorkLogService _logSvc;
        private readonly IEmailSender _emailSender;

        public CountLogAndEmailJob(IUserService userSvc, IWorkLogService logSvc, IEmailSender emailSender)
        {
            _logSvc = logSvc;
            _userSvc = userSvc;
            _emailSender = emailSender;
        }
        //Begin 起始时间；
        //Interval 表示时间间隔，单位是毫秒，此处事24小时；
        //SkipWhileExecuting 是否等待上一个执行任务完成，true为等待
        [Invoke(Begin = "2018-11-29 11:29",Interval = 1000*3600*24,SkipWhileExecuting = true)]
        public void Run(IServiceProvider services)
        {
            //定时任务具体执行的业务逻辑
            //查询


            //统计


            //发送邮件
            //_emailSender.SendEmailAsync("364572026@qq.com", "日志汇总情况", "有几个个人没写日志");
        }
    }
}
