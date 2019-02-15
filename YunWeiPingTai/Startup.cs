using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using YunWeiPingTai.Configuration;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;
using YunWeiPingTai.Service;
using YunWeiPingTai.Services;
using ApplicationUser = YunWeiPingTai.Service.ApplicationUser;

namespace YunWeiPingTai
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTimedJob();//注册TimeJob服务
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region 注册cookie
            //注册cookie认证服务
            services.AddAuthentication(UserAuthorizeAttribute.UserAuthenticationScheme)
                .AddCookie(UserAuthorizeAttribute.UserAuthenticationScheme,options => {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = new PathString("/Error/Forbidden");//没有权限时跳转页面
                });
            #endregion

            #region 注册MyDbContext
            //注册MyDbContext并提供options，就可以在依赖注入中使用了
            var connectionstr = Configuration["connectionStrs:developmentConnectionStr"];
            services.AddEntityFrameworkSqlServer().AddDbContext<MyDbContext>(t => t.UseSqlServer(connectionstr));
            #endregion

            # region 添加Identity服务和所有依赖相关的服务
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MyDbContext>();
            #endregion

            #region 配置redisCache并注册
            var redisConn = Configuration["Redis:Connection"];
            var redisInstanceName= Configuration["Redis:InstanceName"];
            var sessionOutTime = Configuration.GetValue<int>("SessionTimeOut");
            //配置RedisCache
            services.AddDistributedRedisCache(options => {
                options.Configuration = redisConn;
                options.InstanceName = redisInstanceName;
            });
            //添加session并设置过期时间
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(sessionOutTime);
            });
            #endregion

            #region 注册服务
            //使用反射把所有服务接口进行了注入
            var serviceAsm = Assembly.Load(new AssemblyName("YunWeiPingTai.Service"));
            foreach(var serviceType in serviceAsm.GetTypes().Where(t=>typeof(IServiceSupport).IsAssignableFrom(t)&&!t.GetTypeInfo().IsAbstract))
            {
                var interfaceTypes = serviceType.GetInterfaces();
                foreach(var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }
            //services.AddScoped(typeof(IAdminUserService), typeof(AdminUserService));
            #endregion

            #region 注册邮件服务
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,MyDbContext dbContext)
        {

            //向数据库中添加种子数据
            //dbContext.EnsureSeedDataForContext();
            //添加Nlog
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseFileServer();//对下面两个中间件的封装
            //app.UseDefaultFiles();//注意写在useStaticFiles之前
            //app.UseStaticFiles();//使用静态文件
            app.UseCookiePolicy();

            app.UseSession();

            //注意app.UseAuthentication方法一定要放在app.UseMvc方法前面，否者后面就算调用HttpContext.SignInAsync进行用户登录后，使用
            //HttpContext.User还是会显示用户没有登录，并且HttpContext.User.Claims读取不到登录用户的任何信息。
            //这说明Asp.Net OWIN框架中MiddleWare的调用顺序会对系统功能产生很大的影响，各个MiddleWare的调用顺序一定不能反
            app.UseAuthentication();

            app.UseTimedJob();//使用timeJob

            /*
             app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
            */
            app.UseMvcWithDefaultRoute();
        }
    }
}
