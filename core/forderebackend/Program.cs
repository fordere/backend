using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;

namespace forderebackend
{
    public class Program
    {
        public static void Main()
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseModularStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000/")
                .Build()
                .Run();
        }
    }
}