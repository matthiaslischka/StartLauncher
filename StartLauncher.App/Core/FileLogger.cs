using System;
using System.IO;

namespace StartLauncher.App.Core
{
    public class FileLogger
    {
        private static FileLogger _fileLogger;

        private FileLogger()
        {
        }

        public static FileLogger GetInstance()
        {
            return _fileLogger ?? (_fileLogger = new FileLogger());
        }

        public void Log(Exception exception)
        {
            var file = new StreamWriter("errorlog.txt", true);
            file.WriteLine($"[{DateTime.Now}]");
            file.WriteLine(exception);

            file.Close();
        }
    }
}