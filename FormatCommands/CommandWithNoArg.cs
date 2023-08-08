namespace SteeringWheel.FormatCommands
{
    public abstract class CommandWithNoArg : MainCommand
    {
        protected CommandWithNoArg(CommandType subj)
            : base(subj)
        {
        }
    }
}
