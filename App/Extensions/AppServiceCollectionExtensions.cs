using App.AOPs;
using App.Filters;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using FreeSql;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.IService;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Extensions
{
    public static class AppServiceCollectionExtensions
    {
        public static IServiceCollection AddModelDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var str = configuration.GetSection("ConnectionStrings:SqlServer:Model").Value;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("ConnectionStrings:SqlServer:Model", "请配置SqlServer中Model数据库的连接字符串!");
            }
            var sqlServer = new FreeSqlBuilder()
               .UseConnectionString(DataType.SqlServer, str)
               //.UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
               .Build();
            services.AddSingleton(typeof(IFreeSql), sqlServer);
            services.AddScoped<IUnitOfWork>(sp => sqlServer.CreateUnitOfWork());
            return services;
        }
        public static IServiceCollection AddMetaDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var str = configuration.GetSection("ConnectionStrings:SqlServer:Meta").Value;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("ConnectionStrings:SqlServer:Meta", "请配置SqlServer中Meta数据库的连接字符串!");
            }
            var sqlServer = new FreeSqlBuilder()
               .UseConnectionString(DataType.SqlServer, str)
               //.UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
               .Build<IMetaDatabase>();
            services.AddSingleton(typeof(IFreeSql<IMetaDatabase>), sqlServer);
            return services;
        }

        
        public static IServiceCollection AppAddFreeSql(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();
            var str = configuration.GetSection("ConnectionStrings:SqlServer:Model").Value;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("ConnectionStrings:SqlServer:Model", "请配置SqlServer中Model数据库的连接字符串!");
            }
            var mysqlStr = configuration.GetSection("ConnectionStrings:MySql:Model").Value;

            var env = provider.GetService<IHostingEnvironment>();
            var mySqlBuilder = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, mysqlStr);

            if (env.IsDevelopment())
            {
                mySqlBuilder
                    .UseAutoSyncStructure(true)
                    .UseSyncStructureToLower(true);
            }
            var mySql = mySqlBuilder.Build<IMySql>();


            mySql.Aop.ConfigEntityProperty = (s, e) =>
            {
                var attr = e.Property.GetCustomAttribute<ColumnAttribute>(false);
                if (attr != null)
                {
                    if (attr.DbType == "image")
                    {
                        e.ModifyResult.DbType = "blob";
                        e.ModifyResult.IsIgnore = true;
                    }
                }
            };

            if (env.IsDevelopment())
            {
                var sqlServer = new FreeSqlBuilder()
                .UseConnectionString(DataType.SqlServer, str)
                .UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
                .Build();
                services.AddSingleton(typeof(IFreeSql), sqlServer);
                services.AddSingleton(typeof(IFreeSql<IMySql>), mySql);
            }
            else
            {
                services.AddSingleton(typeof(IFreeSql), mySql);
            }


            //services.AddSingleton(typeof(IFreeSql<IDBLog>), new FreeSqlBuilder()
            //    .UseConnectionString(DataType.SqlServer, str)
            //    .UseMonitorCommand(cmd => Trace.WriteLine(cmd.CommandText))
            //    .Build<IDBLog>());
            return services;
        }

        /// <summary>
        /// 注入redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceCollection AppAddCsRedis(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IRedisCacheManager, RedisCacheManager>(); 
            List<string> conns = new List<string>();
            foreach (var item in configuration.GetSection("ConnectionStrings:Redis").GetChildren())
            {
                conns.Add(item.Value);
                break;
            }
            var csredis = new CSRedis.CSRedisClient(null, conns.ToArray());
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

                #region 添加注释文件
                var xmlPath = Path.Combine(basePath, "App.xml");//这个就是刚刚配置的xml文件名
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                }
                //if (File.Exists(xmlPath))
                //{
                //    c.IncludeXmlComments(Path.Combine(basePath, "Data.xml"));
                //}
                #endregion

                #region ids4认证
                //c.AddSecurityDefinition("oauth2", new OAuth2Scheme()
                //{
                //    Flow = "implicit",
                //    AuthorizationUrl = "http://localhost:5000/connect/authorize",
                //    Scopes = new Dictionary<string, string>
                //    {
                //        {"demo_api","swagger_api access" }
                //    }
                //});
                //c.OperationFilter<AuthorizeCheckOperationFilter>(); 
                #endregion

                #region Bearer
                c.AddSecurityDefinition("Bearer",
                            new ApiKeyScheme
                            {
                                In = "header",
                                Description = "请输入OAuth接口返回的Token，前置Bearer。示例：Bearer {Token}",
                                Name = "Authorization",
                                Type = "apiKey"
                            });
                c.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer",
                          Enumerable.Empty<string>()
                        },
                    });
                #endregion
            });
            return services;
        }

        /// <summary>
        /// Cors 跨域
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AppAddCors(this IServiceCollection services, IConfiguration Configuration)
        {
            //var Configuration = services.BuildServiceProvider().GetService<IConfiguration>();
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
                c.AddPolicy("vue.js", policy =>
                {
                    policy.WithOrigins("http://localhost:9192")
                    .AllowAnyHeader()
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

            builder.RegisterType<BillOperationAOP>();
          

            var assemblysServices = Assembly.Load("Shop.Service");//要记得!!!这个注入的是实现类层，不是接口层！不是 IServices
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            //builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();
            //    .EnableInterfaceInterceptors()
            //    .InterceptedBy(typeof(ServiceInterceptorAOP));//指定已扫描程序集中的类型注册为提供所有其实现的接口。
            //var assemblysRepository = Assembly.Load("Internal.Repository.SqlServer");//模式是 Load(解决方案名)
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            //assemblysRepository = Assembly.Load("Internal.Repository.FreeSqlServer");//模式是 Load(解决方案名)
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            //assemblysRepository = Assembly.Load("Admin.Service");//模式是 Load(解决方案名) FreeSql Service
            //builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().EnableInterfaceInterceptors(); 

            // aop 拦截特定的接口 后期可通过配置的方式实现
            var types = assemblysServices.GetTypes().Where(w => {
                if (!w.IsAbstract)
                {
                    foreach (var item in w.GetInterfaces())
                    {
                        if (item.IsGenericType && item.GetGenericTypeDefinition().IsAssignableFrom(typeof(IBaseBillService<,>)))
                        {
                            return true;
                        }
                    }
                }                
                return false;
            }).ToArray();
            if (types!=null && types.Length>0)
            {
                builder.RegisterTypes(types).AsImplementedInterfaces().EnableInterfaceInterceptors().InterceptedBy(typeof(BillOperationAOP));
            } 

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            #endregion

            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }

        /// <summary>
        /// 启用跨域处理
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }

        /// <summary>
        /// 启用自定义的错误处理
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
