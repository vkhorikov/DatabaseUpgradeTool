using System;
using System.Collections.Generic;


namespace DatabaseUpgradeTool
{
    /// <summary>
    /// The theory behind this tool: http://enterprisecraftsmanship.com/2015/08/10/database-versioning-best-practices/
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Press 1 to execute updates");
            if (Console.ReadKey().KeyChar != '1')
                return;

            Console.WriteLine();
            Console.WriteLine();

            List<string> output = new SettingsManager().ExecuteUpdates();
            foreach (string str in output)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
