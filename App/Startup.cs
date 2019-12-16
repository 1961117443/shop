using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            var freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.SqlServer, "server=.;uid=sa;pwd=123456;database=GZMModel_SFY")
                .UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
                .Build();
            //var freeSql2 = new FreeSql.FreeSqlBuilder()
            //    .UseConnectionString(FreeSql.DataType.SqlServer, "server=192.168.31.138;uid=dev;pwd=dev+-*/86224155;database=YongZhen_GZMModel")
            //    .UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
            //    .Build();
            services.AddSingleton(typeof(IFreeSql), freeSql);
            //services.AddSingleton(typeof(IFreeSqlFactory), new FreeSqlFactory());
            //services.AddScoped(typeof(QiniuService));
            //services.AddScoped(typeof(QiniuService));
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region AutoMapper 先注册autoMapper 在使用autofac框架托管

            services.AddAutoMapper(Assembly.Load("App"));
            //AutoMapper.Configuration.MapperConfigurationExpression expression = new AutoMapper.Configuration.MapperConfigurationExpression();
            //expression.AddProfiles(Assembly.Load("Internal.Data"));
            //AutoMapper.Mapper.Initialize(expression);

            #endregion

            #region Swagger

            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.0.0",
                    Title = "Order API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Shop.API", Email = "", Url = "" }
                }); 

                //var xmlPath = Path.Combine(basePath, "Internal.App.xml");//这个就是刚刚配置的xml文件名
                //if (File.Exists(xmlPath))
                //{
                //    c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                //}
                //if (File.Exists(xmlPath))
                //{
                //    c.IncludeXmlComments(Path.Combine(basePath, "Internal.Data.xml"));
                //} 
            });
            #endregion
            #region CORS 跨域
            //跨域第二种方法，声明策略，记得下边app中配置
            services.AddCors(c =>
            {
                //↓↓↓↓↓↓↓注意正式环境不要使用这种全开放的处理↓↓↓↓↓↓↓↓↓↓
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                    .AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });
                //↑↑↑↑↑↑↑注意正式环境不要使用这种全开放的处理↑↑↑↑↑↑↑↑↑↑
                List<string> os = new List<string>();
                var origins = Configuration.GetSection("AllowAnyOrigins");
                if (origins != null)
                {
                    foreach (var cfg in origins.GetChildren())
                    {
                        os.Add($"{cfg.Value}");
                    }
                }
                //一般采用这种方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    .WithOrigins(os.ToArray())//支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });
            //跨域 注意下边 Configure方法 中进行配置
            //services.AddCors();
            #endregion 
            #region AutoFac

            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();

            //注册要通过反射创建的组件
            //builder.RegisterType<DemandService>().As<IDemandService>();

            var assemblysServices = Assembly.Load("Shop.Service");//要记得!!!这个注入的是实现类层，不是接口层！不是 IServices
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            //    .EnableInterfaceInterceptors()
            //    .InterceptedBy(typeof(ServiceInterceptorAOP));//指定已扫描程序集中的类型注册为提供所有其实现的接口。
            //var assemblysRepository = Assembly.Load("Internal.Repository.SqlServer");//模式是 Load(解决方案名)
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            //assemblysRepository = Assembly.Load("Internal.Repository.FreeSqlServer");//模式是 Load(解决方案名)
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            //assemblysRepository = Assembly.Load("Admin.Service");//模式是 Load(解决方案名) FreeSql Service
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().EnableInterfaceInterceptors();

            //  builder.RegisterType<DemandBillOperation>().As<IBillOperation<Demand>>().EnableInterfaceInterceptors();

            //services.AddScoped(typeof(IDemandBillOperation), typeof(DemandBillOperation)).en;

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //注册拦截器
            //builder.RegisterType<ServiceInterceptorAOP>();

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            #endregion

            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllRequests");
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiHelp V2");
            });
            #endregion

            
            app.UseMvc();
        }
    }
}
