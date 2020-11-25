using System;
using System.IO;
using Microsoft.Extensions.Configuration;
public class AppConfiguration
{

    public static IConfigurationRoot Config
    {
        get
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }

    public static IConfigurationSection AppSettings { get => Config.GetSection("db-connection"); }
    public static String Connection { get => AppSettings["connectionstring"]; }

}