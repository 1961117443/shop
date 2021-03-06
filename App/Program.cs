﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("Configs/nlog.config").GetCurrentClassLogger();
            //try
            //{
            //    logger.Debug("init main");
            //    CreateWebHostBuilder(args).Build().Run();
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Stopped program because of exception");
            //    throw;
            //}
            //finally
            //{
            //    NLog.LogManager.Shutdown();
            //}

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                   .AddJsonFile("hosting.json", optional: true)   //增加hosting.json
                   .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                 .UseStartup<Startup>()
             .ConfigureLogging(logging =>
             {
                 //logging.ClearProviders();
                 logging.SetMinimumLevel(LogLevel.Trace);
             })
             .UseNLog(); // 依赖注入 nlog
        }
            
    }
}
