using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace DatabaseVersioningTool
{
    public class VersionManager
    {
        private readonly DBHelper _dbHelper;


        public VersionManager(string connectionString)
        {
            _dbHelper = new DBHelper(connectionString);
        }


        public IReadOnlyList<string> ExecuteMigrations()
        {
            var output = new List<string>();
            int currentVersion = GetCurrentVersion();
            output.Add("Current DB schema version is " + currentVersion);

            IReadOnlyList<Migration> migrations = GetNewMigrations(currentVersion);
            output.Add(migrations.Count + " migration(s) found");

            int? duplicatedVersion = GetDuplicatedVersion(migrations);
            if (duplicatedVersion != null)
            {
                output.Add("Non-unique migration found: " + duplicatedVersion);
                return output;
            }

            foreach (Migration migration in migrations)
            {
                _dbHelper.ExecuteMigration(migration.GetContent());
                UpdateVersion(migration.Version);
                output.Add("Executed migration: " + migration.Name);
            }

            if (!migrations.Any())
            {
                output.Add("No updates for the current schema version");
            }
            else
            {
                int newVersion = migrations.Last().Version;
                output.Add("New DB schema version is " + newVersion);
            }

            return output;
        }


        private int? GetDuplicatedVersion(IReadOnlyList<Migration> migrations)
        {
            int duplicatedVersion = migrations
                .GroupBy(x => x.Version)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .FirstOrDefault();

            return duplicatedVersion == 0 ? (int?)null : duplicatedVersion;
        }


        private void UpdateVersion(int newVersion)
        {
            _dbHelper.ExecuteNonQuery(@"UPDATE dbo.Settings SET Value = @Version WHERE Name = 'Version'",
                new SqlParameter("Version", newVersion.ToString()));
        }


        private IReadOnlyList<Migration> GetNewMigrations(int currentVersion)
        {
            // 01_MyMigration.sql
            var regex = new Regex(@"^(\d)*_(.*)(sql)$");

            return new DirectoryInfo(@"Migrations\")
                .GetFiles()
                .Where(x => regex.IsMatch(x.Name))
                .Select(x => new Migration(x))
                .Where(x => x.Version > currentVersion)
                .OrderBy(x => x.Version)
                .ToList();
        }


        private int GetCurrentVersion()
        {
            if (!SettingsTableExists())
            {
                CreateSettingsTable();
                return 0;
            }

            return GetCurrentVersionFromSettingsTable();
        }


        private int GetCurrentVersionFromSettingsTable()
        {
            string version = _dbHelper.ExecuteScalar<string>("SELECT Value FROM dbo.Settings WHERE Name = 'Version'");
            return int.Parse(version);
        }


        private void CreateSettingsTable()
        {
            string query = @"
                CREATE TABLE dbo.Settings
                (
                    Name nvarchar(50) NOT NULL PRIMARY KEY,
                    Value nvarchar(500) NOT NULL
                )

                INSERT dbo.Settings (Name, Value)
                VALUES ('Version', '0')";

            _dbHelper.ExecuteNonQuery(query);
        }


        private bool SettingsTableExists()
        {
            string query = @"
                IF (OBJECT_ID('dbo.Settings', 'table') IS NULL)
                SELECT 0
                ELSE SELECT 1";

            return _dbHelper.ExecuteScalar<int>(query) == 1;
        }
    }
}
