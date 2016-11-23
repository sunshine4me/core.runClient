using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using core.phoneDevice;
using core.runClient.DataEntities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using core.runClient.Extensions;

namespace core.runClient {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment()) {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //开启静态文件目录浏览
            services.AddDirectoryBrowser();

            //数据库注入
            services.AddDbContext<runClientDbContext>(options => options.UseSqlite("Filename=App_Data/runClientDb.db"));

            //注入DeviceManage
            services.AddSingleton<DeviceManage>();

            //注入自定义配置
            services.AddOptions();
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services
               .AddMvc()
               .AddJsonOptions(r => {//设置json相关参数
                   r.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();//设置默认规则,首字母不强制小写
                   r.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;//忽略null值
                   r.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss";//时间格式化
               });

            var bsp = services.BuildServiceProvider();
            runClient.Task.SmokeTestTask.Provider = bsp;

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {

            //开启静态文件目录浏览
            app.UseDirectoryBrowser(new DirectoryBrowserOptions() {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ResultFile")),
                RequestPath = new PathString("/ResultFile")
            });

            //静态文件查看
            app.UseStaticFiles(new StaticFileOptions() {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"ResultFile")),
                RequestPath = new PathString("/ResultFile")
            });


            loggerFactory.AddConsole(Configuration.GetSection("LoggUseStaticFilesing"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

          
            app.UseStaticFiles();


            //权限相关
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                //CookieHttpOnly = true,
                //ExpireTimeSpan = TimeSpan.FromDays(1),
                AuthenticationScheme = "core.runClient",
                LoginPath = new PathString("/Login"),
                AccessDeniedPath = new PathString("/Login/Forbidden"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                //CookieName = ".mcookie",
                //CookiePath = "/",
            });


            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }



    }
}
