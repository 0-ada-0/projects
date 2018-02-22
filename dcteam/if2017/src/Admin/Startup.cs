using IF2017.Admin.Common;
using IF2017.Admin.Configs;
using IF2017.Admin.Extensions;
using IF2017.Admin.Filter;
using IF2017.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using System;
using System.Text;

namespace IF2017.Admin
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            string basePath = ConfigPathBuilder.GetWebConfigPath();
            var ifbuilder = new ConfigurationBuilder()
               .SetBasePath(basePath)
               .AddJsonFile("ifconfig.json", optional: true, reloadOnChange: true);
            IFConfigReader.SetConf(ifbuilder.Build());
            // 配置redis连接
            RedisHelper.ConnectionString = IFConfigReader.GetValue("config:redisconnection");            
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache>(serviceProvider =>
            new RedisCache(new RedisCacheOptions
            {
                Configuration = IniHelper.LoadIni("../web.config")["connectionStrings"]["IF2017RedisConnectionString"],
                InstanceName = "Session_IF2017"
            })).AddSession();

            services.AddMvc(options =>
            {
                //暂不启用加密安全
                //options.ValueProviderFactories.Insert(0, new DecryptorValueProviderFactory());
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });
            services.AddCors(
                options => options.AddPolicy("zhanmen",p=>p
                .WithOrigins("http://10.17.65.42:8077", "http://ywweb.duoyi.com:8077", "http://ywweb.duoyi.com", "http://121.201.116.14:10077","http://localhost:12700")
                .AllowAnyHeader()
                .AllowAnyMethod())
                );
            #region Swagger插件

            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "IF2017 API",
                    Description = "IF2017 项目webapi接口说明",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "duoyi", Email = "", Url = "http://duoyi.com" },
                    License = new License { Name = "duoyi", Url = "http://duoyi.com" }
                });
                options.IncludeXmlComments(AppContext.BaseDirectory + @"/Admin.xml");
                options.GroupActionsBy(e => e.RelativePath.Contains("/test/") ? $"Test_{e.GroupName}" : e.GroupName);
                options.DocumentFilter<HideInDocsFilter>();
            });
            services.AddSwaggerGen();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding("GB2312");
            Console.InputEncoding = Encoding.GetEncoding("GB2312");

            app.UseCors("zhanmen");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddNLog().AddDebug();

            //NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(GetConfigPath.GetLogConfigPath());
            env.ConfigureNLog(env.IsDevelopment() ? "nlog.Development.config" : "nlog.config");

            if (env.IsDevelopment() || IFConfigReader.IsProg)
            {
                app.UseSwagger().UseSwaggerUi();
            }
            app.UseRequestLogger();
            app.UseMvc();
        }
    }
}
