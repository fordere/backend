using System;

using Mono.Unix;
using Mono.Unix.Native;

using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;

namespace Fordere.WebConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new AppSettings();
            var hostUrl = config.GetRequiredString("API.Url");
            var host = new AppHostConsole();
            host.Init();

            var log = LogManager.GetLogger(typeof(Program));

            Console.WriteLine(@"
        ____               __                              _ 
       / __/___  _________/ /__  ________     ____ _____  (_)
      / /_/ __ \/ ___/ __  / _ \/ ___/ _ \   / __ `/ __ \/ / 
     / __/ /_/ / /  / /_/ /  __/ /  /  __/  / /_/ / /_/ / /  
    /_/  \____/_/   \__,_/\___/_/   \___/   \__,_/ .___/_/   
                                                /_/          
                            by Stefan Schoeb, Oliver Zuercher");

            Console.WriteLine();
            Console.WriteLine("starting on {0}...".Fmt(hostUrl));

            host.Start(hostUrl);

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ready!");
            Console.ForegroundColor = defaultColor;

            //log.Info("REST API started");

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var signals = new[] {new UnixSignal(Signum.SIGINT), new UnixSignal(Signum.SIGTERM)};

                // Wait for a unix signal
                for (bool exit = false; !exit;)
                {
                    int id = UnixSignal.WaitAny(signals);

                    if (id >= 0 && id < signals.Length)
                    {
                        if (signals[id].IsSet) exit = true;
                    }
                }
            }
            else
            {
                Console.ReadLine();
            }
        }
    }
}