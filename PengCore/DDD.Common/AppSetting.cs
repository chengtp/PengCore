﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDD.Common
{
    public class AppSetting
    {
        private static readonly object objLock = new object();
        private static AppSetting instance = null;

        private IConfigurationRoot Config { get; }

        private AppSetting()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Config = builder.Build();
        }

        public static AppSetting GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new AppSetting();
                    }
                }
            }

            return instance;
        }

        public static string GetConfig(string name)
        {
            return GetInstance().Config.GetSection(name).Value;
        }

        public static string GetConnectionString(string database)
        {
            return GetInstance().Config.GetSection("ConnectionStrings").GetSection(database).Value;
        }
    }
}