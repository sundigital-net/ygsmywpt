using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NewLife.Agent;

namespace YunWeiPingTai
{
    public class Program
    {
        protected static string[] _args;
        public static void Main(string[] args)
        {
            //_args = args;
            //MyAgentService.ServiceMain();
            CreateWebHostBuilder(args)//调用方法创建一个IWebHostBuilder对象
                .Build()//用上面返回的对象创建一个IWebHost
                .Run();//运行上面创建的IWebHost对象从而运行我们的Web应用程序换句话说就是启动一个一直运行监听http请求的任务
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000")
                .UseKestrel()
                .UseUrls("http://*:5000");//监听5000端口
        public class MyAgentService:AgentServiceBase<MyAgentService>
        {
            #region 属性

            public override string DisplayName => "YWPT.AgentService";
            public override string Description => "运维平台作为windows服务来启动";

            #endregion

            #region 构造函数

            public MyAgentService()
            {
                ServiceName = "YWPT.AgentService";
            }

            #endregion

            #region 任务执行
            
            protected override void StartWork(string reason)
            {
                CreateWebHostBuilder(_args).Build().Run();
                WriteLog("当前时间{0}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                base.StartWork(reason);
            }

            #endregion
        }
    }
}
