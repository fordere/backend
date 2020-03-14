using System;
using System.Linq;
using System.Reflection;

using DbUp;

using ServiceStack.Configuration;

namespace Fordere.DbMigration
{
    public static class DbMigration
    {
        public static void Main(string[] args)
        {
            var appSettings = new AppSettings();
            var connectionString = args.FirstOrDefault() ??
                                   string.Format("Server = {0}; Port = {1}; Database = {2}; Uid = {3}; Pwd = {4}",
                                       appSettings.Get("DB.Host"),
                                       appSettings.Get("DB.Port", 3306),
                                       appSettings.Get("DB.Name"),
                                       appSettings.Get("DB.User"),
                                       appSettings.Get("DB.Pass"));

            var upgrader =
                DeployChanges.To
                    .MySqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("press any key to quit");
            Console.ReadLine();
        }
    }
}
