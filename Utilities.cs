namespace ProblematicBookmark
{
    using Newtonsoft.Json;
    using System;

    internal static class Utilities
    {
        public static void Save(object obj)
        {
            var root = Directory.GetCurrentDirectory();
            if (string.IsNullOrEmpty(root))
            {
                throw new Exception("root path is null");
            }

            var newdirPath = Path.Combine(Path.Combine(root, "ScanResult"));
            if (!Directory.Exists(newdirPath))
            {
                Directory.CreateDirectory(newdirPath);
            }

            using (StreamWriter file = File.CreateText(Path.Combine(newdirPath, "FilterResult_" + Guid.NewGuid() + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, obj);
            }
        }
    }
}
