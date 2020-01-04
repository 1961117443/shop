﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using FreeSql;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.IService;
using Shop.Service;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Extensions
{
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 注入FreeSql服务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AppAddFreeSql(this IServiceCollection services)
        { 
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var str = configuration.GetSection("ConnectionStrings:SqlServer:Model").Value;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) )
            {
                throw new ArgumentNullException("ConnectionStrings:SqlServer:Model", "请配置SqlServer中Model数据库的连接字符串!");
            }
            var dataFreeSql = new FreeSqlBuilder()
                .UseConnectionString(DataType.SqlServer, str)
                .UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
                .Build();
            //var dblogFreeSql = new FreeSqlBuilder()
            //    .UseConnectionString(DataType.SqlServer, "server=.;uid=sa;pwd=123456;database=GZMModel_SFY")
            //    .UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
            //    .Build();
            //services.AddSingleton(typeof(IFreeSql), dblogFreeSql);
            services.AddSingleton(typeof(IFreeSql), dataFreeSql);
            return services;
        }

        /// <summary>
        /// 注入redis
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AppAddCsRedis(this IServiceCollection services)
        {
            //services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var str = configuration.GetConnectionString("Redis");
            var csredis = new CSRedis.CSRedisClient(str);
            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            return services;
        }


        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AppAddSwagger(this IServiceCollection services)
        { 
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
            return services;
        }

        /// <summary>
        /// Cors 跨域
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AppAddCors(this IServiceCollection services)
        {
            var Configuration = services.BuildServiceProvider().GetService<IConfiguration>();
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
            return services;
        }

        /// <summary>
        /// AutoFac
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider AppAddAutoFac(this IServiceCollection services)
        {
            #region AutoFac

            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();

            //注册要通过反射创建的组件
            //builder.RegisterType<DemandService>().As<IDemandService>();

            var assemblysServices = Assembly.Load("Shop.Service");//要记得!!!这个注入的是实现类层，不是接口层！不是 IServices
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();
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

           return  new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }
    }
}