using System;
using System.Collections.Generic;
using System.Configuration;


namespace DatabaseVersioningTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["Main"];
            if (connectionString == null)
            {
                Console.WriteLine("Please add a connection string with the 'Main' key");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Press 1 to execute updates");
            if (Console.ReadKey().KeyChar != '1')
                return;

            Console.WriteLine();
            Console.WriteLine();

            IReadOnlyList<string> output = new VersionManager(connectionString.ConnectionString).ExecuteMigrations();
            foreach (string str in output)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
