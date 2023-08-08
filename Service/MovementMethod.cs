using SteeringWheel.Connection;

namespace SteeringWheel.Service
{
    public static class MovementMethod
    {
        public static void SelectedMovementMethod(string key)
        {
            switch (key)
            {
                case "Up":
                    GoToYPlus();
                    break;
                case "Left":
                    GoToXMinus();
                    break;
                case "Down":
                    GoToYMinus();
                    break;
                case "Right":
                    GoToXPlus();
                    break;
                case "Shift+Up":
                    GoToZPlus();
                    break;
                case "Shift+Down":
                    GoToZMinus();
                    break;
            }
        }
        public static void GoToXMinus()
        {
            var command = Commands.CommandXMinus(BroadcastStepValue.GetStepValue());
            SerialPortConnection.Sender(command);
        }
        public static void GoToXPlus()
        {
            var command = Commands.CommandXPlus(BroadcastStepValue.GetStepValue());
            SerialPortConnection.Sender(command);
        }
        public static void GoToYMinus()
        {
            var command = Commands.CommandYMinus(BroadcastStepValue.GetStepValue());
            SerialPortConnection.Sender(command);
        }
        public static void GoToYPlus() 
        {
            var command = Commands.CommandYPlus(BroadcastStepValue.GetStepValue());
            SerialPortConnection.Sender(command);
        }
        public static void GoToZMinus()
        {
            var command = Commands.CommandZMinus(BroadcastStepValue.GetStepValue());
            SerialPortConnection.Sender(command);
        }
        public static void GoToZPlus()
        {
            var command = Commands.CommandZPlus(BroadcastStepValue.GetStepValue());
            SerialPortConnection.Sender(command);
        }
    }
}
