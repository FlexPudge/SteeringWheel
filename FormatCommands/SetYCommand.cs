namespace SteeringWheel.FormatCommands
{
    public class SetYCommand : CommandWithArg
    {
        public SetYCommand(int arg) : base(CommandType.SetY, arg)
        { }
    }
}
