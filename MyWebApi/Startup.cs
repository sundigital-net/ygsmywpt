using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyWebApi.Entity;
using MyWebApi.IService;
using MyWebApi.Model;
using MyWebApi.Service;
using NLog.Extensions.Logging;

namespace MyWebApi
{
    //其实Startup才算是程序真正的切入点
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }//注入IConfiguration
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //ConfigureServices把services（各种服务，例如identity，ef，mvc等等）注册到container（asp .net core的容器）中去，
        //并配置这些services. 这个container是用来进行dependency injection的(依赖注入). 
        //所有注入的services(此外还包括一些框架已经注册好的services) 在以后写代码的时候, 都可以将它们注入(inject)进去. 
        //例如Configure方法的参数, app, env, loggerFactory都是注入进去的services.
        public void ConfigureServices(IServiceCollection services)
        {
            //注册服务，三种方法AddTransient，AddScoped和AddSingleton，这些都表示service的生命周期。
            /*
            transient的services是每次请求（不是指Http request）都会创建一个新的实例，它比较适合轻量级的无状态的（Stateless）的service。

            scope的services是每次http请求会创建一个实例。

            singleton的在第一次请求的时候就会创建一个实例，以后也只有这一个实例，或者在ConfigureServices这段代码运行的时候创建唯一一个实例。
             */

            //注册MyDbContext并提供options，就可以在依赖注入中使用了
            //使用SQLServer server=实例名称；database=数据库名称；Trusted_Connection=True表示这是一个受信任的连接，也就是说使用了继集成验证，windows下就是指的windows凭证。
            //var connectionstr = @"server=localhost\SQLEXPRESS;database=MyWebApi;Trusted_Connection=True";

            //数据库连接字符串可以保存到环境变量中，详见---https://www.cnblogs.com/cgzl/p/7661805.html
            var connectionstr = Configuration["connectionStrs:developmentConnectionStr"];
            //使用MySql
            services.AddDbContext<MyDbContext>(t=>t.UseMySQL(connectionstr));//需要Nuget   MySql.Data.EntityFrameworkCore

            #if DEBUG
            services.AddTransient<IMailService,LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            services.Configure<Content>(Configuration.GetSection("Content"));

            //注册
            services.AddScoped<IProductService, ProductService>();

            //01.注册并使用Mvc
            services.AddMvc()
                //asp.net core 默认只实现了json的返回
                //修改配置，添加Xml格式
                .AddMvcOptions(options=> {
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        //Configure 是asp.net core程序用来具体指定如何处理每个http请求的，例如我们可以让程序知道我们使用mvc来处理http请求，
        //那就调用app.UserMvc()这个方法，但是目前所有的http请求都会返回“hello world”
        //执行顺序  Progra.Main()-->ConfigureServices()-->Configure()
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory ,MyDbContext dbContext)
        {
            /*
             请求管道: 那些处理http request并返回responses的代码就组成了request pipeline(请求管道).
             中间件: 我们可以做的就是使用一些程序来配置那些请求管道 request pipeline以便处理requests和responses. 
                    比如处理验证(authentication)的程序, 连MVC本身就是个中间件(middleware).
             每层中间件接到请求后都可以直接返回或者调用下一个中间件. 一个比较好的例子就是: 
                    在第一层调用authentication验证中间件, 如果验证失败, 那么直接返回一个表示请求未授权的response.
             */

            //添加种子数据，自己写的
            dbContext.EnsureSeedDataForContext();


            //添加Nlog
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                //一个middleware, 当exception发生的时候, 这段程序就会处理它.
                //而判断env.isDevelopment() 表示, 这个middleware只会在Development环境下被调用.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //正式环境中需要捕获并记录exception，需要使用这个middleware
                app.UseExceptionHandler();
            }
            
            
            //使用状态码中间件
            app.UseStatusCodePages();
            //02.使用Mvc中间件
            app.UseMvc();

            /*
            app.Run(async (context) =>
            {
                //int i = 0,j=10;
                //int z = j / i;
                await context.Response.WriteAsync("Hello World!");
            });
            */
        }
    }
}
