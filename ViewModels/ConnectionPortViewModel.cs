using SteeringWheel.Connection;
using SteeringWheel.Service;
using SteeringWheel.Service.Buttons;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SteeringWheel.ViewModels
{
    public class ConnectionPortViewModel : ViewModelBase
    {
        private string? _selectedPort;
        private int? _selectedBaudRate;
        private ObservableCollection<string>? _portName;
        private ICommand? connectedCommand;
        private ObservableCollection<int>? _baudRate = new ObservableCollection<int>() {9600,115200 };

        public Action<bool>? Notify { get; internal set; }
        public ObservableCollection<string>? PortName
        {
            get => _portName;
            set => _portName = value;
        }
        public ObservableCollection<int>? BaudRate
        {
            get => _baudRate;
            set => _baudRate = value;
        }
        public string? SelectedPort
        {
            get { return _selectedPort!; }
            set { _selectedPort = value; }
        }
        public int? SelectedBaudRate
        {
            get { return _selectedBaudRate!; }
            set { _selectedBaudRate = value; }
        }
        public ICommand? ConnectedCommand
        {
            get
            {
                return connectedCommand
                    ?? (connectedCommand = new ActionCommand(() =>
                    {
                        ConnectionPort();
                    }));
            }
        }
        public ConnectionPortViewModel() => LoadPortName();
        private void LoadPortName()
        {
            PortName = new ObservableCollection<string>();
            var portsName = SerialPort.GetPortNames();
            foreach (var portName in portsName)
            {
                PortName.Add(portName);
            }
        }
        private void ConnectionPort()
        {
            if (SelectedPort == null)
            {
                MessageBox.Show("Выберите ComPort");
                return;
            }
            if (SelectedPort != null)
            {
                SerialPortConnection.Connection(SelectedPort, SelectedBaudRate);

            }
            SerialPortConnection.Sender(Commands.CommandE2());
            Notify?.Invoke(SerialPortConnection.StatusConnection());
        }
    }
}
