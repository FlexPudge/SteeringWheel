namespace SteeringWheel.FormatCommands
{
    public class SetXCommand : CommandWithArg
    {
        public SetXCommand(int arg) : base(CommandType.SetX, arg)
        {
        }
    }
}
