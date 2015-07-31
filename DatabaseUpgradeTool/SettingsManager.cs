using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace DatabaseUpgradeTool
{
    public class SettingsManager
    {
        private readonly DBHelper _dbHelper;


        public SettingsManager()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            _dbHelper = new DBHelper(connectionString);
        }


        public List<string> ExecuteUpdates()
        {
            var output = new List<string>();

            int version = GetCurrentVersion();
            output.Add("Current DB schema version is " + version);

            List<UpdateFile> updates = GetUpdates(version);
            output.Add(updates.Count + " update(s) found");
            
            foreach (UpdateFile update in updates)
            {
                _dbHelper.ExecuteSchemaUpdate(update.GetContent());
                UpdateVersion(update.Version);
                output.Add("Executed update: " + update.Name);
            }

            if (!updates.Any())
            {
                output.Add("No updates for current schema version");
            }
            else
            {
                int newVersion = updates.Last().Version;
                output.Add("Current DB schema version is " + newVersion);
            }

            return output;
        }


        private List<UpdateFile> GetUpdates(int version)
        {
            var regex = new Regex(@"^(\d)*_(.*)(sql)$");

            return new DirectoryInfo(@"Scripts\")
                .GetFiles()
                .Where(x => regex.IsMatch(x.Name))
                .Select(x => new UpdateFile(x))
                .Where(x => x.Version > version)
                .OrderBy(x => x.Version)
                .ToList();
        }


        private int GetCurrentVersion()
        {
            if (!SettingsTableExists())
            {
                CreateSettingsTable();
                UpdateVersion(1);

                return 1;
            }

            return GetCurrentVersionFromSettingTable();
        }


        private bool SettingsTableExists()
        {
            return _dbHelper.ExecuteScalar<int>("IF (OBJECT_ID('dbo.Settings', 'table') IS NULL) SELECT 0 ELSE SELECT 1") == 1;
        }


        private void CreateSettingsTable()
        {
            string text = @"
                CREATE TABLE dbo.Settings
                (
                    Name nvarchar(50) NOT NULL PRIMARY KEY,
                    Value nvarchar(500) NOT NULL
                )

                INSERT dbo.Settings (Name, Value)
                VALUES ('Version', '-')";

            _dbHelper.ExecuteNonQuery(text);
        }


        private void UpdateVersion(int newVersion)
        {
            _dbHelper.ExecuteNonQuery(@"UPDATE dbo.Settings SET Value = @Version WHERE Name = 'Version'",
                new SqlParameter("Version", newVersion.ToString()));
        }


        private int GetCurrentVersionFromSettingTable()
        {
            string version = _dbHelper.ExecuteScalar<string>("SELECT Value FROM dbo.Settings WHERE Name = 'Version'");
            return int.Parse(version);
        }
    }
}
