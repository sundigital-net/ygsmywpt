using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using YunWeiPingTai.Custom;
using YunWeiPingTai.IService;
using YunWeiPingTai.Models;
using YunWeiPingTai.Service;
using YunWeiPingTai.Services;

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

            #region 防跨站请求（XSRF/CSRF）攻击处理

            services.AddAntiforgery(options =>
            {
                options.FormFieldName= "AntiforgeryKey_sundigital";
                options.HeaderName = "X-CSRF-TOKEN-sundigital";
                options.SuppressXFrameOptionsHeader = false;
            });

            #endregion

            #region 注册cookie身份认证服务
            //注册cookie认证服务
            services.AddAuthentication(b =>
                {
                    b.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    b.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    b.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = new PathString("/Error/Forbidden");//没有权限时跳转页面
                    options.Cookie.Name = "My_SessionId";//cookie名字
                    options.Cookie.Expiration=new TimeSpan(0,15,0);

                });
            #endregion

            #region 注册MyDbContext
            //注册MyDbContext并提供options，就可以在依赖注入中使用了
            var connectionstr = Configuration["connectionStrs:developmentConnectionStr"];
            services.AddEntityFrameworkSqlServer().AddDbContext<MyDbContext>(t => t.UseSqlServer(connectionstr));
            #endregion

            # region 添加Identity服务和所有依赖相关的服务--停用
            /*
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionstr, b => b.MigrationsAssembly(nameof(YunWeiPingTai))));
            services.AddDefaultIdentity<IdentityUser>()
                .AddUserValidator<MyUserValidator<IdentityUser>>()//添加自定义的用户验证器,中文
                .AddEntityFrameworkStores<IdentityDbContext>();
                
            //移除默认的验证器,必须添加到AddDefaultIdentity之后
            var service = services.FirstOrDefault(t => t.ImplementationType == typeof(UserValidator<IdentityUser>));
            if (service != null)
            {
                services.Remove(service);
            }

            services.Configure<IdentityOptions>(options =>
            {
                //密码
                options.Password.RequireDigit = true;//数字
                options.Password.RequireLowercase = true;//小写
                options.Password.RequireNonAlphanumeric = false;//特殊字符
                options.Password.RequireUppercase = false;//大写
                options.Password.RequiredLength = 8;//最小长度

                // 锁定设置
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);//账户锁定时长30分钟
                options.Lockout.MaxFailedAccessAttempts = 5;//5次失败的尝试将账户锁定

                //User设置
                //options.User.AllowedUserNameCharacters =
                //     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                options.User.RequireUniqueEmail = true;//邮箱唯一
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789{中}";


                // Cookie常用设置
                //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);//Cookie 保持有效的时间150天。
                //options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";//在进行登录时自动重定向。
                //options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";//在进行注销时自动重定向。
            });
            */
            #endregion

            #region 配置redisCache、session并注册
            var redisConn = Configuration["Redis:Connection"];
            var redisInstanceName = Configuration["Redis:InstanceName"];
            var sessionOutTime = Configuration.GetValue<int>("SessionTimeOut");
            //配置RedisCache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisConn;
                options.InstanceName = redisInstanceName;
            });
            //添加session并设置过期时间
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(sessionOutTime);
                options.Cookie.HttpOnly = true;
            });
            #endregion

            #region 注册服务
            //使用反射把所有服务接口进行了注入
            var serviceAsm = Assembly.Load(new AssemblyName("YunWeiPingTai.Service"));
            foreach (var serviceType in serviceAsm.GetTypes().Where(t => typeof(IServiceSupport).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            {
                var interfaceTypes = serviceType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, MyDbContext dbContext)
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
