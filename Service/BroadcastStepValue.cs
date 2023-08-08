namespace SteeringWheel.Service
{
    internal static class BroadcastStepValue
    {
        static double _stepValue { get; set; }
        public static void SetStepValue(double stepValue) => _stepValue = stepValue;
        public static double GetStepValue() => _stepValue;

    }
}
