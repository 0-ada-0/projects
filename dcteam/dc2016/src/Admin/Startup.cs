using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NLog.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using Microsoft.Extensions.Caching.Redis;
using DC2016.Admin.Configs;
using DC2016.Admin.DC2;
using Microsoft.AspNetCore.Http;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.IO;
using DC2016.Admin.Controllers.Urs;
using DC2016.Admin.Extensions;
using DC2016.Admin.Filter;
using DC2016.Admin.DC;
using DC2016.BLL;
using Swashbuckle.SwaggerGen.Generator;

namespace DC2016.Admin
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
            AppConf.Initialize(this.Configuration);

            string basePath = GetConfigPath.GetWebConfigPath();
            //DC2配置文件
            builder = new ConfigurationBuilder()
               .SetBasePath(basePath)
               .AddJsonFile("dc2.json", optional: true, reloadOnChange: true);
            DC2Conf.Initialize(builder.Build());

            // 配置MySQL连接
            DAL.SqlHelper.ConnectionString = DC2Conf.MySqlConnection;
            // 配置redis连接
            RedisHelper.ConnectionString = DC2Conf.RedisConnection;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //分布式缓存
            services.AddSingleton<IDistributedCache>(serviceProvider => new RedisCache(new RedisCacheOptions
            {
                Configuration = IniHelper.LoadIni("../web.config")["connectionStrings"]["DC2016RedisConnectionString"],
                InstanceName = "Session_DC2016"
            })).AddSession();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "DC2016 API",
                    Description = "DC2016 项目webapi接口说明",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "duoyi", Email = "", Url = "http://duoyi.com" },
                    License = new License { Name = "duoyi", Url = "http://duoyi.com" }
                });
                options.IncludeXmlComments(AppContext.BaseDirectory + @"/Admin.xml");
                options.GroupActionsBy(e => e.RelativePath.Contains("/test/") ? $"Test_{e.GroupName}" : e.GroupName);
                options.DocumentFilter<HideInDocsFilter>();
            });
            services.AddSwaggerGen();

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddTransient<ViewRenderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding("GB2312");
            Console.InputEncoding = Encoding.GetEncoding("GB2312");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddNLog().AddDebug();

            //NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(GetConfigPath.GetLogConfigPath());
            env.ConfigureNLog(env.IsDevelopment() ? "nlog.Development.config" : "nlog.config");

            DAL.SqlHelper.Instance.Log = loggerFactory.CreateLogger("DC2016_DAL_sqlhelper");
            if (env.IsDevelopment() || DC2Conf.IsProg)
            {
                app.UseSwagger().UseSwaggerUi();
            }

            //app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            app.UseRequestLogger();
            app.UseMvc();

            app.UseDefaultFiles().UseStaticFiles(); //UseDefaultFiles 必须在 UseStaticFiles 之前调用
        }
    }
}
