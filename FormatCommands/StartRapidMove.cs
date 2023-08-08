namespace SteeringWheel.FormatCommands
{
    public class StartRapidMove : CommandWithArg
    {
        public StartRapidMove(int arg) : base(CommandType.Rapid, arg)
        {
        }
    }
}
