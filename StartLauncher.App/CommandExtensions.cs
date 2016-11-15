using System.IO;

namespace StartLauncher.App
{
    public static class CommandExtensions
    {
        public static FileInfo GetCommandFileInfo(this CommandDto command)
        {
            var commandFileName = command.Name + ".bat";
            var commandRelativePath = Path.Combine("Commands", commandFileName);
            return new FileInfo(commandRelativePath);
        }
    }
}