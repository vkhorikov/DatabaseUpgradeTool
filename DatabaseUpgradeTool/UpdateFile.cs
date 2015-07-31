using System;
using System.IO;
using System.Linq;


namespace DatabaseUpgradeTool
{
    internal class UpdateFile
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public int Version { get; private set; }


        public UpdateFile(FileInfo fileInfo)
        {
            string version = fileInfo.Name.Split('_').First();
            Version = int.Parse(version);
            Name = fileInfo.Name;
            Path = fileInfo.FullName;
        }


        public string GetContent()
        {
            return File.ReadAllText(Path);
        }
    }
}
