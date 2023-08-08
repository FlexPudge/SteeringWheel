namespace SteeringWheel.FormatCommands
{
    public class SpindleSpeedCommand : CommandWithArg
    {
        public SpindleSpeedCommand(int arg) : base(CommandType.SetSpindleSpeed, arg)
        {
        }
    }
}
