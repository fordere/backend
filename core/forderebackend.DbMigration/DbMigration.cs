using System;
using System.Linq;
using System.Reflection;
using DbUp;

namespace forderebackend.DbMigration
{
    public static class DbMigration
    {
        public static void Main(string[] args)
        {
            var connectionString = args.FirstOrDefault() ??
                                   string.Format("Server = {0}; Port = {1}; Database = {2}; Uid = {3}; Pwd = {4}",
                                       "localhost",
                                       3306,
                                       "fordere",
                                       "root",
                                       "root");

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