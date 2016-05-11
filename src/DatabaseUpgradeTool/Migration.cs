using System.IO;
using System.Linq;


namespace DatabaseVersioningTool
{
    internal class Migration
    {
        public string Name { get; private set; }
        public string FilePath { get; private set; }
        public int Version { get; private set; }


        public Migration(FileInfo fileInfo)
        {
            string version = fileInfo.Name.Split('_').First();
            Version = int.Parse(version);
            Name = fileInfo.Name;
            FilePath = fileInfo.FullName;
        }


        public string GetContent()
        {
            return File.ReadAllText(FilePath);
        }
    }
}
