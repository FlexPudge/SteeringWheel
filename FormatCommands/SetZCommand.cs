namespace SteeringWheel.FormatCommands
{
    public class SetZCommand : CommandWithArg
    {
        public SetZCommand(int arg) : base(CommandType.SetZ, arg)
        {
        }
    }
}
