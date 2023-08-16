using SteeringWheel.Service;
using System;
using System.IO.Ports;

namespace SteeringWheel.Connection
{
    internal static class SerialPortConnection
    {
        private static SerialPort? _serialPort;
        static bool statusConnection = false;
        public static void Connection(string name, int baudRate)
        {
            if (_serialPort?.IsOpen == true)
            {
                statusConnection = true;
                return;
            }
            if (_serialPort == null)
            {
                _serialPort = new SerialPort();
                _serialPort.PortName = name;
                _serialPort.BaudRate = baudRate;
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Handshake = Handshake.None;
                _serialPort.Open();
                statusConnection = true;
                return;
            }
        }
        public static bool StatusConnection() => statusConnection;
        public static byte[] Response()
        {
            byte[] buffer = new byte[0];
            if (statusConnection == true)
            {
                int bufferSize = _serialPort!.BytesToRead;
                buffer = new byte[bufferSize];
                int count = 0;
                for (int i = 0; i < bufferSize; i++)
                {
                    byte bt = (byte)_serialPort.ReadByte();
                    if (0xFF == bt)
                    { }
                    buffer[count] = bt;
                    count++;
                }
                return buffer;
            }
            return buffer;
        }
        public static void Sender(byte[] message)
        {
            if (_serialPort?.IsOpen == true)
                _serialPort.Write(message, 0, message.Length);
        }
    }
}
