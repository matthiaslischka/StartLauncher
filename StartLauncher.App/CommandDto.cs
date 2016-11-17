using System;

namespace StartLauncher.App
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

        public override string ToString()
        {
            return Name;
        }
    }
}