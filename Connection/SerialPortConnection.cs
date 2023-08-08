using SteeringWheel.Service;
using System;
using System.IO.Ports;

namespace SteeringWheel.Connection
{
    internal static class SerialPortConnection
    {
        private static SerialPort? _serialPort;
        static bool statusConnection = false;
        public static void Connection(string name, TypeConnection typeConnection)
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
                switch (typeConnection)
                {
                    case TypeConnection.ComPort:
                        _serialPort.BaudRate = 115200;
                        break;
                    case TypeConnection.Bluetooth:
                        _serialPort.BaudRate = 9600;
                        break;

                }
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
            if (statusConnection == true) 
            {
                int bufferSize = _serialPort.BytesToRead;
                byte [] buffer = new byte[bufferSize];
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
            return null;
        }
        public static void Sender(byte[] message)
        {
            if (_serialPort?.IsOpen == true)
                _serialPort.Write(message, 0, message.Length);
        }
    }
}
