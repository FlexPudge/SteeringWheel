namespace SteeringWheel.FormatCommands
{
    public abstract class CommandWithArg : MainCommand
    {
        protected CommandWithArg(CommandType subj, int arg)
            : base(subj, arg)
        {
        }
    }
}
