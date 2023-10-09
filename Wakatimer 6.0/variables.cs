using System;
using Microsoft.Extensions.Configuration;


public class Variables
{
    public string api_host { set; get; } = "https://wakatime.com/api/v1/";

    public Variables()
    {
        main();
    }

    static void main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

        string api_host = configuration["api_host"];

    }
}

