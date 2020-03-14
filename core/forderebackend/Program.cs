using System;
using System.IO;
using System.Reflection;
using forderebackend.ServiceInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Funq;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Text;

namespace forderebackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseModularStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000/")
                .Build();

            host.Run();
        }
    }
}