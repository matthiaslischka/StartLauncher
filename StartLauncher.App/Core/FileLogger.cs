using System;
using System.IO;

namespace StartLauncher.App.Core
{
    public class FileLogger
    {
        private FileLogger()
        {
        }

        public static FileLogger Current { get; } = new FileLogger();

        public void Log(Exception exception)
        {
            var file = new StreamWriter("errorlog.txt", true);
            file.WriteLine($"[{DateTime.Now}]");
            file.WriteLine(exception);

            file.Close();
        }
    }
}