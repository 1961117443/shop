using App.Common;
using App.Extensions;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Shop.Common;
using Shop.Common.Utils;
using Shop.ViewModel;
using System;
using System.Reflection;
using System.Text;

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
            services.AddSingleton(typeof(CustomExpressionHelper));
            //services.AddScoped(typeof(QiniuService));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
            //services.AddScoped<IDataTranService, DataTranService>();
            #endregion

            #region 注入appsettings.json的Configure
            // token 设置
            IConfigurationSection section = Configuration.GetSection("tokenManagement");
            services.Configure<TokenManagement>(section);
            var token = section.Get<TokenManagement>();

            #endregion

            #region bearer
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                        ValidIssuer = token.Issuer,
                        ValidAudience = token.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }); 
            #endregion

            //用户校验
            #region ids4
            //services.AddAuthorization()
            //     .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //     .AddIdentityServerAuthentication(options =>
            //     {
            //         options.Authority = "http://localhost:5000"; // IdentityServer服务器地址
            //      options.RequireHttpsMetadata = false; // 指定是否为HTTPS

            //      // options.ApiName = "demo_api"; // 用于针对进行身份验证的API资源的名称 swagger
            //      options.ApiName = "shop.api";
            //         options.ApiSecret = "api1pwd";  //对应ApiResources中的密钥
            //     }); 
            #endregion

            #region 扩展服务 AutoMapper 先注册autoMapper 在使用autofac框架托管
            services.AddAutoMapper(Assembly.Load("App"));
            services.AppAddCsRedis(Configuration);
            services.AppAddSwagger();
            //.AppAddCors(Configuration);
            #endregion
            #region 添加sqlserver
            services.AddModelDatabase(Configuration)
                .AddMetaDatabase(Configuration);
            #endregion
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("any", builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials();
            //    });
            //});

            services.AddMvc(options=> 
            {
                options.Filters.Add(new AuthorizeFilter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
            



            return services.AppAddAutoFac();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.UseCors("any");
            // app.UseCors("AllRequests");
            app.UseCorsMiddleware();

            app.UseAuthentication();
            //app.UseAuthorization();

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

            //app.UseAuthorization();

            app.UseMvc();
        }
    }
}
