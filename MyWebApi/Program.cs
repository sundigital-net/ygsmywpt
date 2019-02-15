using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MyWebApi
{
    //程序入口
    //asp .net core application实际就是控制台程序，他是一个调用asp .net core 相关库的控制台程序
    public class Program
    {
        //main方法的内容是配置和运行程序的
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();//程序启动时，会调用Startup这个类
    }
}
