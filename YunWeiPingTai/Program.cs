using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace YunWeiPingTai
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)//调用方法创建一个IWebHostBuilder对象
                .Build()//用上面返回的对象创建一个IWebHost
                .Run();//运行上面创建的IWebHost对象从而运行我们的Web应用程序换句话说就是启动一个一直运行监听http请求的任务
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000");//监听5000端口
    }
}
