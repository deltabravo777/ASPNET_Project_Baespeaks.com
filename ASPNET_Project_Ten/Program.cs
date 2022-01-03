using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASPNET_Project_Eleven
{
    // To Do List:
    //
    // [~] Find a way to center the navbar without breaking it
    // [~] carousel-inner has a fixed height of 500px. this makes it look weird on phone screens
    // [ ] categories for articles
    // [ ] hyperlink in body
    // [ ] images as background
    // [ ] connection string built in the Startup class

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

