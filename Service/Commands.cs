using System;

namespace SteeringWheel.Service
{
    internal static class Commands
    {
        private static byte[] command = new byte[7];
        const byte _asciiE = 101;
        const byte _startSymbol = 35;
        const byte _endSymbol = 74;
        private static UInt16 CheckSumUint16(byte[] message)
        {
            UInt16 sum = 0;
            for (int i = 2; i <= message.Length - 2; i++)
            {
                sum = (ushort)(sum + message[i]);
                sum = (ushort)(sum ^ (sum >> 2));
            }
            sum = (ushort)(sum & 0x000000FF);
            return sum;
        }
        public static byte[] CommandE2()
        {
            byte[] _protocolE2Command = new byte[5];
            _protocolE2Command[0] = _startSymbol;
            _protocolE2Command[1] = 0;
            _protocolE2Command[2] = _asciiE;
            _protocolE2Command[3] = 2;
            _protocolE2Command[4] = _startSymbol;

            return _protocolE2Command;
        }

        public static byte[] CommandXPlus(double value)
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 100; // x+
            command[4] = Convert.ToByte(Convert.ToUInt32(value * 10) & 0x0000FF);
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);

            return command;
        }
        public static byte[] CommandXMinus(double value)
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 101; // x-
            command[4] = Convert.ToByte(Convert.ToUInt32(value * 10) & 0x0000FF);
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);
            return command;
        }
        public static byte[] CommandYMinus(double value)
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 103; // y-
            command[4] = Convert.ToByte(Convert.ToUInt32(value * 10) & 0x0000FF);
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);
            return command;
        }
        public static byte[] CommandYPlus(double value)
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 102; // y+
            command[4] = Convert.ToByte(Convert.ToUInt32(value * 10) & 0x0000FF);
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);
            return command;
        }
        public static byte[] CommandZMinus(double value)
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 105; // z-
            command[4] = Convert.ToByte(Convert.ToUInt32(value * 10) & 0x0000FF);
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);
            return command;
        }
        public static byte[] CommandZPlus(double value)
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 104; // z+
            command[4] = Convert.ToByte(Convert.ToUInt32(value * 10) & 0x0000FF);
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);
            return command;
        }
        public static byte[] LoadCoordinats()
        {
            command[0] = _startSymbol;
            command[1] = 0;
            command[2] = _asciiE;
            command[3] = 8;
            command[4] = 0;
            command[5] = 0;
            command[6] = _endSymbol;

            command[1] = (byte)CheckSumUint16(command);
            return command;
        }
    }
}
