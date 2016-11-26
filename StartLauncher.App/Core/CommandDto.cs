using System;

namespace StartLauncher.App.Core
{
    public class CommandDto
    {
        public CommandDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public string Description { get; set; }
        public bool RunAsAdmin { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}