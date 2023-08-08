using SteeringWheel.Service;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows.Input;
using System.Windows;
using SteeringWheel.Service.Buttons;
using SteeringWheel.Service.RadioButton;
using SteeringWheel.Connection;
using System.Threading;

namespace SteeringWheel.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _statusConnection;
        private string? _selectedPort;
        private double _progressBarValue;
        private ObservableCollection<string> _portName;

        #region координаты
        private double _coordValueX;
        private double _coordValueY;
        private double _coordValueZ;
        private double _coordValueA;

        public double CoordValueX
        {
            get
            {
                if (_coordValueX == null)
                {
                    _coordValueX = 0;
                    return _coordValueX;
                }
                return _coordValueX;
            }
            set
            {
                _coordValueX = value;
                OnPropertyChanged("CoordValueX");
            }
        }
        public double CoordValueY
        {
            get
            {
                if (_coordValueY == null)
                {
                    _coordValueY = 0;
                    return _coordValueY;
                }
                return _coordValueY;
            }
            set
            {
                _coordValueY = value;
                OnPropertyChanged("CoordValueY");
            }
        }
        public double CoordValueZ
        {
            get
            {
                if (_coordValueZ == null)
                {
                    _coordValueZ = 0;
                    return _coordValueZ;
                }
                return _coordValueZ;
            }
            set
            {
                _coordValueZ = value;
                OnPropertyChanged("CoordValueZ");
            }
        }
        public double CoordValueA
        {
            get
            {
                if (_coordValueA == null)
                {
                    _coordValueA = 0;
                    return _coordValueA;
                }
                return _coordValueA;
            }
            set
            {
                _coordValueA = value;
                OnPropertyChanged("CoordValueA");
            }
        }
        #endregion

        public string StatusConnection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_statusConnection))
                {
                    _statusConnection = "Нет подключения";
                }
                return _statusConnection;
            }
            set
            {
                _statusConnection = value;
                OnPropertyChanged("StatusConnection");
            }
        }
        public string SelectedPort
        {
            get { return _selectedPort!; }
            set { _selectedPort = value; }
        }
        public double ProgressBarValue
        {
            get => _progressBarValue;
            set
            {
                _progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
                BroadcastStepValue.SetStepValue(ProgressBarValue);
            }
        }
        public ObservableCollection<string> PortName
        {
            get => _portName;
            set => _portName = value;
        }

        private ICommand? connectedPortCommand;
        public ICommand ConnectedPortCommand
        {
            get
            {
                return connectedPortCommand
                    ?? (connectedPortCommand = new ActionCommand(() =>
                    {
                        ConnectionPort();
                    }));
            }
        }

        private ICommand? updateCoordinats;
        public ICommand UpdateCoordinats
        {
            get
            {
                return updateCoordinats
                    ?? (updateCoordinats = new ActionCommand(() =>
                    {
                        UpdateCoordinates();
                    }));
            }
        }

        private ICommand? keyDetectedCommand;
        public ICommand KeyDetectedCommand
        {
            get
            {
                this.keyDetectedCommand = this.keyDetectedCommand ?? new ActionCommand
                    (x => SelectedKey(x.ToString()));
                return this.keyDetectedCommand;
            }
        }

        #region скрытые комманды
        private ICommand? addPosition;
        public ICommand AddPosition
        {
            get
            {
                return addPosition
                    ?? (addPosition = new ActionCommand(() =>
                    {
                        SavePositionInFile.AddPositionFiles(CoordValueX,
                            CoordValueY,CoordValueZ);
                    }));
            }
        }

        private ICommand? savePosition;
        public ICommand SavePosition
        {
            get
            {
                return savePosition
                    ?? (savePosition = new ActionCommand(() =>
                    {
                        SavePositionInFile.SaveFiles();
                    }));
            }
        }
        #endregion

        private TypeConnection enumRadio;
        public TypeConnection EnumRadio
        {
            get => enumRadio;
            set
            {
                enumRadio = value;
            }
        }

        public MainViewModel()
        {
            LoadPortName();
        }
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
                switch (EnumRadio)
                {
                    case TypeConnection.ComPort:
                        SerialPortConnection.Connection(SelectedPort, TypeConnection.ComPort);
                        break;
                    case TypeConnection.Bluetooth:
                        SerialPortConnection.Connection(SelectedPort, TypeConnection.Bluetooth);
                        break;
                }
            }
            SerialPortConnection.Sender(Commands.CommandE2());
            CheckStatusConnection();
        }
        private void CheckStatusConnection()
        {
            bool conn = SerialPortConnection.StatusConnection();
            if (conn)
            {
                StatusConnection = "Есть подключение";
            }
        }
        private void UpdateCoordinates()
        {
            SerialPortConnection.Sender(Commands.LoadCoordinats());
            Thread.Sleep(500);
            ConvertCoordinates();
        }
        private void ConvertCoordinates()
        {
            int count = 0;
            var response = SerialPortConnection.Response();
            if (response == null)
            {
                MessageBox.Show("Нет подключения");
                return;
            }
            if (response.Length != null)
            {
                byte[] coord = new byte[2];
                ObservableCollection<double> Coordinates = new ObservableCollection<double>();
                for (int i = 2; i < response.Length - 1; i++)
                {
                    coord[count] = response[i];
                    count++;
                    if (count == 2)
                    {
                        var num = BitConverter.ToUInt16(coord, 0);
                        Coordinates.Add(num / 8);
                        count = 0;
                    }

                }
                CoordValueA = Coordinates[0];
                CoordValueX = Coordinates[1];
                CoordValueY = Coordinates[2];
                CoordValueZ = Coordinates[3];
            }
        }

        private void SelectedKey(string key)
        {
            MovementMethod.SelectedMovementMethod(key);
        }
    }
}
