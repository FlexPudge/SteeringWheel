using System.Collections.Generic;
using System.Linq;

namespace SteeringWheel.FormatCommands
{
    public abstract class MainCommand
    {
        static Dictionary<CommandType, string> prefixes =
           new Dictionary<CommandType, string>();

        protected byte[] bytes;
        public CommandType Subj { get; }
        public int? Argument;

        protected MainCommand(CommandType subj,
        int? arg = null)
        {
            Subj = subj;
            Argument = arg;

            if (!prefixes.Any())
            {
                prefixes.Add(CommandType.SetX, "X");
                prefixes.Add(CommandType.SetY, "Y");
                prefixes.Add(CommandType.SetZ, "Z");
                prefixes.Add(CommandType.SetA, "A");
                prefixes.Add(CommandType.Rapid, "M");
                prefixes.Add(CommandType.SetSpindleSpeed, "S");
            }
        }
        public override string ToString()
        {
            return Subj.ToString();
        }
    }
}
