using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Cache;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shop.Common.Utils;
using Shop.IService;
using Shop.Service;
using Swashbuckle.AspNetCore.Swagger;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 注入自己的服务 
            //services.Configure<TokenOptions>(Configuration.GetSection("AuthTokenOptions"));
            //services.AddSingleton(typeof(JwtToken));
            services.AddSingleton(typeof(CustomExpressionHelper));
            //services.AddSingleton(typeof(IFreeSqlFactory), new FreeSqlFactory());
            //services.AddScoped(typeof(QiniuService));
            //services.AddScoped(typeof(QiniuService));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDataTranService, DataTranService>();
            #endregion

            //用户校验
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
              .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = "http://localhost:5000"; // IdentityServer服务器地址
                  options.ApiName = "demo_api"; // 用于针对进行身份验证的API资源的名称
                  options.RequireHttpsMetadata = false; // 指定是否为HTTPS
              });

            #region 扩展服务 AutoMapper 先注册autoMapper 在使用autofac框架托管
            services.AddAutoMapper(Assembly.Load("App"))
                    .AppAddFreeSql()
                    .AppAddCsRedis()
                    .AppAddSwagger()
                    .AppAddCors(); 
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); 

            //services.AddAuthorization()
            //        .AddAuthentication("Bearer")
            //        .AddIdentityServerAuthentication(options =>
            //        {
            //            options.Authority = "http://localhost:5000";
            //            options.RequireHttpsMetadata = false;
            //            options.ApiName = "api1";
            //            //options.ApiSecret = "shop.api";
            //        });

            return services.AppAddAutoFac();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllRequests");

            app.UseAuthentication();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.OAuthClientId("demo_api_swagger");
                c.OAuthAppName("Demo API - Swagger-演示");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiHelp V2");
            });
            #endregion

            

            app.UseMvc();
        }
    }
}
