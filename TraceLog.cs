namespace ProblematicBookmark
{
    using System;
    using System.IO;
    using System.Text;

    internal class TraceLog : IDisposable
    {
        private StringBuilder sb = new StringBuilder();
        private static string traceLogName = Guid.NewGuid().ToString();
        public static void WriteLineInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void WriteLineError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }

        public TraceLog LogLineError(string message)
        {
            sb.AppendLine(message);
            return this;
        }

        public void Dispose()
        {
            var str = sb.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "ScanResult");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileAdd(Path.Combine(path, $"tracelog_{traceLogName}.txt"), sb.ToString());
            }

            sb = null;
        }

        private static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

    }
}
